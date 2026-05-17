// DocumentHostMdiDemoForm.cs
// Phase 09 — runnable acceptance target for the design-time setup wizard.
//
// What this demo proves end-to-end (runtime equivalent of the
// drop-once-wizard-Apply pipeline shipped in Phase 07 + 08):
//   1. The DocumentSetupWizardDialog can be hosted at runtime and its
//      DocumentSetupResult drives a real BeepDocumentManager (which view
//      to attach, whether to seed sample documents, which layout template).
//   2. BeepTabbedView (over BeepDocumentHost) and BeepNativeMdiView are
//      both reachable from one chosen mode without manual rewiring.
//   3. The Beep theme system flows into the wizard via BeepThemesManager
//      so the wizard chrome matches the host application — this validates
//      the WizardPalette adapter shipped in Phase 08.
//
// Launch with: WinFormsApp.UI.Test.exe --demo document-host-mdi
//
// Cross-references:
//   - DocumentSetupWizardDialog          (TheTechIdea.Beep.Winform.Controls.Design.Server)
//   - WizardPalette                      (TheTechIdea.Beep.Winform.Controls.Design.Server)
//   - BeepDocumentHost / BeepDocumentManager (TheTechIdea.Beep.Winform.Controls)
//   - .plans/DocumentHost-MDI-Phase-09-RuntimeWizardDemo.md
// ─────────────────────────────────────────────────────────────────────────────
using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls.Design.Server.Dialogs;
using TheTechIdea.Beep.Winform.Controls.DocumentHost;

namespace WinFormsApp.UI.Test
{
    /// <summary>
    /// Sample form that hosts a <see cref="BeepDocumentManager"/>+
    /// <see cref="BeepDocumentHost"/>+<see cref="BeepTabbedView"/> trio
    /// and lets the user re-run the
    /// <see cref="DocumentSetupWizardDialog"/> to swap modes / seed sample
    /// tabs at runtime. It is the runtime acceptance target for the Phase
    /// 07–08 design-time wizard work.
    /// </summary>
    internal sealed class DocumentHostMdiDemoForm : Form
    {
        // ── Components ────────────────────────────────────────────────────
        private readonly BeepDocumentHost    _host;
        private readonly BeepDocumentManager _manager;
        private BeepTabbedView?              _tabbedView;
        private BeepNativeMdiView?           _mdiView;

        // ── Chrome ────────────────────────────────────────────────────────
        private readonly ToolStrip _toolStrip;
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _modeLabel;
        private readonly ToolStripStatusLabel _docCountLabel;

        public DocumentHostMdiDemoForm()
        {
            Text          = "DocumentHost — Setup Wizard Runtime Demo";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize   = new Size(900, 620);
            Size          = new Size(1180, 760);
            Font          = new Font("Segoe UI", 9f);

            // Accessibility — mirrors the conventions used by the
            // CustomCaptionRegionDemoForm.
            AccessibleName        = "Document Host MDI Demo";
            AccessibleDescription = "Demonstrates the BeepDocumentHost setup wizard, mode switching, and document seeding at runtime.";
            AccessibleRole        = AccessibleRole.Window;

            // ── Status strip ──────────────────────────────────────────────
            _statusStrip = new StatusStrip
            {
                AccessibleName = "Demo status"
            };
            _modeLabel = new ToolStripStatusLabel("Mode: —")
            {
                Spring     = true,
                TextAlign  = ContentAlignment.MiddleLeft,
                AccessibleName = "Current display mode"
            };
            _docCountLabel = new ToolStripStatusLabel("Documents: 0")
            {
                AccessibleName = "Open document count"
            };
            _statusStrip.Items.Add(_modeLabel);
            _statusStrip.Items.Add(_docCountLabel);

            // ── Toolbar ───────────────────────────────────────────────────
            _toolStrip = new ToolStrip
            {
                GripStyle      = ToolStripGripStyle.Hidden,
                RenderMode     = ToolStripRenderMode.System,
                AccessibleName = "Demo actions"
            };
            _toolStrip.Items.Add(MakeButton(
                "Run Setup Wizard…",
                "Re-open the design-time setup wizard and apply its result to this manager.",
                (s, e) => RunWizard(autoTriggered: false)));
            _toolStrip.Items.Add(new ToolStripSeparator());
            _toolStrip.Items.Add(MakeButton(
                "Add Document",
                "Append a new document with a generated title.",
                (s, e) => AddOneDocument()));
            _toolStrip.Items.Add(MakeButton(
                "Close All",
                "Close every open document on the active view.",
                (s, e) => _manager.CloseAllDocuments()));

            // ── DocumentHost + Manager (default = tabbed) ─────────────────
            _host = new BeepDocumentHost
            {
                Dock = DockStyle.Fill,
                AccessibleName = "Document area"
            };

            _manager = new BeepDocumentManager
            {
                ThemeName = string.Empty
            };
            _tabbedView      = new BeepTabbedView { Host = _host };
            _manager.View    = _tabbedView;

            _manager.DocumentAdded += (s, e) => UpdateStatus();

            // ── Compose ───────────────────────────────────────────────────
            Controls.Add(_host);
            Controls.Add(_toolStrip);
            Controls.Add(_statusStrip);
            _toolStrip.Dock   = DockStyle.Top;
            _statusStrip.Dock = DockStyle.Bottom;

            UpdateStatus(initialMode: DocumentSetupMode.TabbedDocuments);

            // First-launch auto-trigger so the demo immediately exercises
            // the wizard once. Subsequent invocations come from the toolbar.
            BeginInvoke((Action)(() => RunWizard(autoTriggered: true)));
        }

        // ══════════════════════════════════════════════════════════════════
        // Wizard pipeline
        // ══════════════════════════════════════════════════════════════════

        private void RunWizard(bool autoTriggered)
        {
            var initialMode = _manager.View switch
            {
                BeepNativeMdiView => DocumentSetupMode.NativeMdi,
                _                 => DocumentSetupMode.TabbedDocuments
            };

            using var dlg = new DocumentSetupWizardDialog(
                initialMode,
                existingDocumentCount: _manager.DocumentCount,
                hostOptions: null);

            var result = dlg.ShowDialog(this);
            if (result != DialogResult.OK || dlg.Result.ConfigureLater)
            {
                if (autoTriggered)
                {
                    _modeLabel.Text = $"Mode: {ModeName(initialMode)} (wizard skipped — use toolbar)";
                }
                return;
            }

            ApplySetupResult(dlg.Result);
        }

        private void ApplySetupResult(DocumentSetupResult setup)
        {
            // 1. View swap if the mode changed
            switch (setup.Mode)
            {
                case DocumentSetupMode.TabbedDocuments:
                case DocumentSetupMode.BrowserTabs:
                    EnsureTabbedView(setup.Mode == DocumentSetupMode.BrowserTabs);
                    break;

                case DocumentSetupMode.NativeMdi:
                    EnsureMdiView();
                    break;
            }

            // 2. Seed sample documents only on a fresh / explicitly-requested run
            if (setup.AddSampleDocuments && setup.SampleDocumentCount > 0)
            {
                SeedSamples(setup.SampleDocumentCount);
            }

            UpdateStatus(initialMode: setup.Mode);
        }

        private void EnsureTabbedView(bool browserStyle)
        {
            if (_mdiView != null)
            {
                IsMdiContainer = false;
                _mdiView = null;
            }

            if (_tabbedView == null || !ReferenceEquals(_manager.View, _tabbedView))
            {
                _tabbedView   = new BeepTabbedView { Host = _host };
                _manager.View = _tabbedView;
            }

            // Phase 10 (DOCMDI-NEXT-019): use the new SetTabStylePreset API
            // that ships parity with BeepDisplayContainer.SetTabStylePreset.
            // The preset bundle covers TabStyle + ShowAddButton +
            // CloseButtonMode in one call, so the demo no longer has to
            // poke at three properties via reflection.
            if (browserStyle)
            {
                _manager.ApplyBrowserPreset();
            }
            else
            {
                _manager.ApplyIdePreset();
            }
        }

        private void EnsureMdiView()
        {
            if (_tabbedView != null && ReferenceEquals(_manager.View, _tabbedView))
            {
                // We don't dispose the tabbed view here — keeping it around
                // lets the user re-enter Tabbed mode without re-creating
                // the document tree from scratch.
                _manager.View = null;
            }

            _mdiView = new BeepNativeMdiView
            {
                ParentForm = this
            };
            _mdiView.DocumentFormCreated += (s, e) =>
            {
                e.Form.Size      = new Size(420, 280);
                e.Form.BackColor = SystemColors.Window;
                e.Form.Controls.Add(new Label
                {
                    Text      = $"MDI child for '{e.Title}'\nId: {e.Id}",
                    Dock      = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font      = new Font("Segoe UI", 10f)
                });
            };
            _manager.View = _mdiView;
        }

        private void SeedSamples(int count)
        {
            _manager.BeginBatchAddDocuments();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    int index = _manager.DocumentCount + 1;
                    _manager.AddDocument($"Document {index}", iconPath: null, activate: i == 0);
                }
            }
            finally
            {
                _manager.EndBatchAddDocuments();
            }
            UpdateStatus();
        }

        // ══════════════════════════════════════════════════════════════════
        // Helpers
        // ══════════════════════════════════════════════════════════════════

        private void AddOneDocument()
        {
            int index = _manager.DocumentCount + 1;
            _manager.AddDocument($"Document {index}");
            UpdateStatus();
        }

        private void UpdateStatus(DocumentSetupMode? initialMode = null)
        {
            DocumentSetupMode resolvedMode = initialMode ?? (_manager.View switch
            {
                BeepNativeMdiView => DocumentSetupMode.NativeMdi,
                _                 => DocumentSetupMode.TabbedDocuments
            });

            _modeLabel.Text    = $"Mode: {ModeName(resolvedMode)}";
            _docCountLabel.Text = $"Documents: {_manager.DocumentCount}";
        }

        private static string ModeName(DocumentSetupMode mode) => mode switch
        {
            DocumentSetupMode.TabbedDocuments => "Tabbed Documents",
            DocumentSetupMode.BrowserTabs     => "Browser Tabs",
            DocumentSetupMode.NativeMdi       => "Native MDI",
            _                                 => mode.ToString()
        };

        private static ToolStripButton MakeButton(string text, string tooltip, EventHandler onClick)
        {
            var b = new ToolStripButton(text)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                ToolTipText  = tooltip,
                AutoSize     = true
            };
            b.Click += onClick;
            return b;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _manager?.Dispose();
                _tabbedView?.Dispose();
                _mdiView?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
