        public void PaintQuickSelectionButtons(Graphics g, Rectangle buttonAreaBounds, DateTimePickerProperties properties, DateTimePickerHoverState hoverState)
        {
            var buttons = new[]
            {
                (properties.TodayButtonText, DateTime.Today),
                (properties.TomorrowButtonText, DateTime.Today.AddDays(1)),
                ("Next Week", DateTime.Today.AddDays(7)),
                ("Next Month", DateTime.Today.AddMonths(1))
            };