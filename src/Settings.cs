using ModSettings;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArrowModRenewed.src
{
    internal enum YesNo
    {
        No,
        Yes
    }

    internal class ArrowModRenewedSettings : JsonModSettings
    {
        private readonly Dictionary<string, (string displayName, object? initialValue, object? latestValue)> _pendingChanges = new();

        private static object? FormatValue(FieldInfo field, object? value)
        {
            if (value != null && field.FieldType.IsEnum)
            {
                return Enum.ToObject(field.FieldType, value);
            }
            return value;
        }

        [Section("Arrow Mod Renewed Settings")]

        [Name("Arrowhead crafting time:")]
        [Description("Minutes to craft (2) arrowheads. Vanilla is 60, recommended is 20-40.")]
        [Slider(1, 120)]
        public int arrowHeadCraftTime = 60;

        [Name("Arrow crafting time:")]
        [Description("Minutes to craft (1) arrow. Vanilla is 90, recommended is 30-60.")]
        [Slider(1, 180)]
        public int arrowCraftTime = 90;

        [Name("Workbench required for arrows?")]
        [Description("Is a workbench required for crafting arrows?\nVanilla = Yes, recommended = player preference!")]
        public YesNo arrowsNoBench = YesNo.Yes;

        [Name("... additional arrow crafting time:")]
        [Description("Additional time in minutes per arrow when crafting without a workbench.")]
        [Slider(15, 60)]
        public int arrowNoBenchTime = 30;

        [Name("Craft Arrow Shafts from Hardwood?")]
        [Description("Allow using hardwood to craft arrow shafts.")]
        public YesNo craftArrowFromWood = YesNo.No;

        [Name("... archery skill level needed?")]
        [Description("Archery skill required to craft arrow shafts from hardwood (Fir). Intended for mid-late game (recommended: 3).")]
        [Slider(0, 5)]
        public int craftArrowFromWoodLevel = 3;

        [Section("     ")]

        [Name("Reset to defaults?")]
        [Description("Set to Yes to reset all settings above to their default values.")]
        public YesNo resetDefaults = YesNo.No;

        private void RecordPendingChange(string fieldName, string displayName, object? oldVal, object? newVal)
        {
            if (_pendingChanges.TryGetValue(fieldName, out var existing))
            {
                _pendingChanges[fieldName] = (displayName, existing.initialValue, newVal);
            }
            else
            {
                _pendingChanges[fieldName] = (displayName, oldVal, newVal);
            }
        }

        private void ApplyDefaults()
        {
            RecordPendingChange(nameof(arrowsNoBench), "Workbench required for arrows?", arrowsNoBench, YesNo.Yes);
            arrowsNoBench = YesNo.Yes;

            RecordPendingChange(nameof(arrowNoBenchTime), "... additional arrow crafting time:", arrowNoBenchTime, 30);
            arrowNoBenchTime = 30;

            RecordPendingChange(nameof(arrowHeadCraftTime), "Arrowhead crafting time:", arrowHeadCraftTime, 60);
            arrowHeadCraftTime = 60;

            RecordPendingChange(nameof(arrowCraftTime), "Arrow crafting time:", arrowCraftTime, 90);
            arrowCraftTime = 90;

            RecordPendingChange(nameof(craftArrowFromWood), "Craft Arrow Shafts from Hardwood?", craftArrowFromWood, YesNo.No);
            craftArrowFromWood = YesNo.No;

            RecordPendingChange(nameof(craftArrowFromWoodLevel), "... archery skill level needed?", craftArrowFromWoodLevel, 3);
            craftArrowFromWoodLevel = 3;

            resetDefaults = YesNo.No;
        }

        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            string displayName = field.GetCustomAttribute<NameAttribute>()?.Name ?? field.Name;
            object? formattedOld = FormatValue(field, oldValue);
            object? formattedNew = FormatValue(field, newValue);

            if (field.Name == nameof(resetDefaults))
            {
                return;
            }

            RecordPendingChange(field.Name, displayName, formattedOld, formattedNew);
            Draw();
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();

            if (resetDefaults == YesNo.Yes)
            {
                _pendingChanges.Clear();
                ApplyDefaults();
                Save();
                Draw();
            }

            foreach (var entry in _pendingChanges)
            {
                string fieldName = entry.Key;
                var (displayName, initialValue, latestValue) = entry.Value;

                if (!Equals(initialValue, latestValue))
                {
                    ArrowModRenewed.LogYellow($"Setting '{displayName}' ({fieldName}) changed: {initialValue} -> {latestValue}");
                }
            }

            _pendingChanges.Clear();
        }
        
        internal void Draw()
        {
            SetFieldVisible(nameof(arrowNoBenchTime), arrowsNoBench == YesNo.No);
            SetFieldVisible(nameof(craftArrowFromWoodLevel), craftArrowFromWood == YesNo.Yes);
            RefreshGUI();
        }
    }

    internal static class Settings
    {
        public static ArrowModRenewedSettings options = new();

        public static void OnLoad()
        {
            options = new ArrowModRenewedSettings();
            options.Draw();
            options.AddToModSettings("Arrow Mod Renewed Settings");
        }
    }
}