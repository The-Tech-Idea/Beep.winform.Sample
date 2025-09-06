using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Base.Helpers;
using TheTechIdea.Beep.Winform.Controls.Converters;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Winform.Controls.Base
{
    // Material rendering extension for BaseControl (partial)
    public partial class BaseControl
    {
        #region Material fields
        // Keep only the essential internal fields for StylePreset functionality
        private MaterialTextFieldStylePreset _stylePreset = MaterialTextFieldStylePreset.Default;
        #endregion

        #region Material properties - Only StylePreset

        // Important: expose only the preset property - it will control all other Material properties
        [Browsable(true)]
        [Category("Material Design")]
        [Description("Applies a predefined style preset that configures variant, density, radius, fill, and helper/label behavior.")]
        [DefaultValue(MaterialTextFieldStylePreset.Default)]
        public MaterialTextFieldStylePreset StylePreset
        {
            get => _stylePreset;
            set
            {
                if (_stylePreset == value) return;
                _stylePreset = value;
                ApplyStylePreset(_stylePreset);
            }
        }

        // HIDDEN legacy Material properties (kept for runtime behavior only)
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool EnableMaterialStyle { get => _bcEnableMaterialStyle; set { _bcEnableMaterialStyle = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public MaterialTextFieldVariant MaterialVariant { get => _bcMaterialVariant; set { _bcMaterialVariant = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public int MaterialBorderRadius { get => _bcMaterialRadius; set { _bcMaterialRadius = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool MaterialShowFill { get => _bcShowFill; set { _bcShowFill = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public Color MaterialFillColor { get => _bcFillColor; set { _bcFillColor = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public Color MaterialOutlineColor { get => _bcOutlineColor; set { _bcOutlineColor = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public Color MaterialPrimaryColor { get => _bcPrimaryColor; set { _bcPrimaryColor = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool MaterialUseVariantPadding { get => _bcUseVariantPadding; set { _bcUseVariantPadding = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public Padding MaterialCustomPadding { get => _bcCustomMaterialPadding; set { _bcCustomMaterialPadding = value; UpdateMaterialLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public string LeadingIconPath { get => _bcLeadingIconPath; set { _bcLeadingIconPath = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public string TrailingIconPath { get => _bcTrailingIconPath; set { _bcTrailingIconPath = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public string LeadingImagePath { get => _bcLeadingImagePath; set { _bcLeadingImagePath = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public string TrailingImagePath { get => _bcTrailingImagePath; set { _bcTrailingImagePath = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool ShowClearButton { get => _bcShowClearButton; set { _bcShowClearButton = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool LeadingIconClickable { get => _bcLeadingIconClickable; set { _bcLeadingIconClickable = value; } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool TrailingIconClickable { get => _bcTrailingIconClickable; set { _bcTrailingIconClickable = value; } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public int IconSize { get => _bcIconSize; set { _bcIconSize = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public int IconPadding { get => _bcIconPadding; set { _bcIconPadding = value; _materialHelper?.UpdateLayout(); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public string ErrorText { get => _bcErrorText; set { _bcErrorText = value; _bcHasError = !string.IsNullOrEmpty(value); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool HasError { get => _bcHasError; set { _bcHasError = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public Color ErrorColor { get => _bcErrorColor; set { _bcErrorColor = value; Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public int MaterialElevationLevel { get => _bcElevationLevel; set { _bcElevationLevel = value; _materialHelper?.SetElevation(_bcElevationLevel); Invalidate(); } }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)] public bool MaterialUseElevation { get => _bcUseElevation; set { _bcUseElevation = value; _materialHelper?.SetElevationEnabled(_bcUseElevation); Invalidate(); } }
        #endregion

        #region Preset application
        public void ApplyStylePreset(MaterialTextFieldStylePreset preset)
        {
            switch (preset)
            {
                case MaterialTextFieldStylePreset.MaterialOutlined:
                    MaterialBorderVariant = MaterialTextFieldVariant.Outlined;
                    BorderRadius = 8;
                    ShowFill = false;
                    break;
                case MaterialTextFieldStylePreset.MaterialFilled:
                    MaterialBorderVariant = MaterialTextFieldVariant.Filled;
                    BorderRadius = 8;
                    ShowFill = true;
                    FilledBackgroundColor = Color.FromArgb(0xEE, 0xEA, 0xF0);
                    break;
                case MaterialTextFieldStylePreset.MaterialStandard:
                    MaterialBorderVariant = MaterialTextFieldVariant.Standard;
                    BorderRadius = 4;
                    ShowFill = false;
                    break;
                case MaterialTextFieldStylePreset.PillOutlined:
                    MaterialBorderVariant = MaterialTextFieldVariant.Outlined;
                    BorderRadius = Math.Max(Height / 2, 20);
                    ShowFill = false;
                    break;
                case MaterialTextFieldStylePreset.PillFilled:
                    MaterialBorderVariant = MaterialTextFieldVariant.Filled;
                    BorderRadius = Math.Max(Height / 2, 20);
                    ShowFill = true;
                    FilledBackgroundColor = Color.FromArgb(245, 245, 245);
                    break;
                case MaterialTextFieldStylePreset.DenseOutlined:
                    MaterialBorderVariant = MaterialTextFieldVariant.Outlined;
                    BorderRadius = 6;
                    ShowFill = false;
                    break;
                case MaterialTextFieldStylePreset.DenseFilled:
                    MaterialBorderVariant = MaterialTextFieldVariant.Filled;
                    BorderRadius = 6;
                    ShowFill = true;
                    FilledBackgroundColor = Color.FromArgb(245, 245, 245);
                    break;
                case MaterialTextFieldStylePreset.ComfortableOutlined:
                    MaterialBorderVariant = MaterialTextFieldVariant.Outlined;
                    BorderRadius = 10;
                    ShowFill = false;
                    break;
                case MaterialTextFieldStylePreset.ComfortableFilled:
                    MaterialBorderVariant = MaterialTextFieldVariant.Filled;
                    BorderRadius = 10;
                    ShowFill = true;
                    FilledBackgroundColor = Color.FromArgb(245, 245, 245);
                    break;
                case MaterialTextFieldStylePreset.Default:
                default:
                    MaterialBorderVariant = MaterialTextFieldVariant.Outlined;
                    BorderRadius = 8;
                    ShowFill = false;
                    break;
            }

            _materialHelper?.UpdateLayout();
            Invalidate();
        }

        #endregion

        #region Partial hook implementation
        partial void DrawCustomBorder_Ext(Graphics g)
        {
            if (_bcEnableMaterialStyle)
            {
                _materialHelper ??= new BaseControlMaterialHelper(this);
                _materialHelper.UpdateLayout();
            }
        }

        #endregion
    }
}