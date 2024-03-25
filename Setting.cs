using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using System.Collections.Generic;
using Unity.Entities;

namespace SchoolCapacityBalancer;

[FileLocation(nameof(SchoolCapacityBalancer))]
[SettingsUIGroupOrder(CapacityGroup, OtherOptionsGroup)]
[SettingsUIShowGroupName(CapacityGroup)]
public class Setting : ModSetting
{
    public const string CapacitySection = "Modify School Capacity";
    public const string ResetSection = "Reset";
    public const string CapacityGroup = "Student Capacity";
    public const string OtherOptionsGroup = "Additional Options";

    public Setting(IMod mod) : base(mod)
    {
        if (ElementarySlider == 0) SetDefaults();
    }

    public override void Apply()
    {
        base.Apply();
        var system = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SchoolCapacityChangerSystem>();
        system.Enabled = true;
    }

    [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
    [SettingsUISection(CapacitySection, CapacityGroup)]
    public int ElementarySlider { get; set; }

    [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
    [SettingsUISection(CapacitySection, CapacityGroup)]
    public int HighSchoolSlider { get; set; }

    [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
    [SettingsUISection(CapacitySection, CapacityGroup)]
    public int CollegeSlider { get; set; }

    [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
    [SettingsUISection(CapacitySection, CapacityGroup)]
    public int UniversitySlider { get; set; }

    private bool _scaleUpkeepWithCapacity = true;
    [SettingsUISection(CapacitySection, CapacityGroup)]
    [SettingsUIHidden]
    public bool ScaleUpkeepWithCapacity { get => _scaleUpkeepWithCapacity; 
        set
        {
            _scaleUpkeepWithCapacity = value;
            Extraneous = !value;
        }
    }

    [SettingsUISection(CapacitySection, ResetSection)]
    [SettingsUIButton]
    public bool ResetToVanilla 
    {
        set 
        {
            SetToVanilla();
            Extraneous = !Extraneous;
            Apply();
        }
    }

    [SettingsUISection(CapacitySection, ResetSection)]
    [SettingsUIButton]
    public bool ResetToModDefault
    {
        set
        {
            SetDefaults();
            Extraneous = !Extraneous;
            Apply();
        }
    }

    [SettingsUIHidden]
    public bool Extraneous { get; set; } = false;

    public override void SetDefaults()
    {
        ElementarySlider = 200;
        HighSchoolSlider = 100;
        CollegeSlider = 125;
        UniversitySlider = 125;
        ScaleUpkeepWithCapacity = true;
        Extraneous = false;
    }

    public void SetToVanilla()
    {
        ElementarySlider = 100;
        HighSchoolSlider = 100;
        CollegeSlider = 100;
        UniversitySlider = 100;
        ScaleUpkeepWithCapacity = true;
    }
}

public class LocaleEN : IDictionarySource
{
    private readonly Setting m_Setting;
    public LocaleEN(Setting setting)
    {
        m_Setting = setting;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
    {
        return new Dictionary<string, string>
        {
            { m_Setting.GetSettingsLocaleID(), "School Capacity Balancer" },
            { m_Setting.GetOptionTabLocaleID(Setting.CapacitySection), "Main" },
            { m_Setting.GetOptionGroupLocaleID(Setting.CapacityGroup), "Student Capacity" },
            { m_Setting.GetOptionGroupLocaleID(Setting.OtherOptionsGroup), "Additional Options" },

            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ElementarySlider)), "Elementary" },
            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HighSchoolSlider)), "High School" },
            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CollegeSlider)), "College" },
            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.UniversitySlider)), "University" },

            { m_Setting.GetOptionDescLocaleID(nameof(Setting.ElementarySlider)), "Set the student capacity of elementary school buildings relative to their vanilla capacity." },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.HighSchoolSlider)), "Set the student capacity of high school buildings relative to their vanilla capacity." },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.CollegeSlider)), "Set the student capacity of college buildings relative to their vanilla capacity." },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.UniversitySlider)), "Set the student capacity of univesity buildings relative to their vanilla capacity." },

            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)), "Scale upkeep with capacity" },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)), "If enabled, school upkeep costs will be scaled with the capacity setting.\nChanging this value requires restarting the game." },

            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToVanilla)), "Reset to Vanilla" },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToVanilla)), "Resets all capacities to 100%." },

            { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToModDefault)), "Reset to Balanced Defaults" },
            { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToModDefault)), "Resets all capacities to the mod's balanced defaults." }
        };
    }

    public void Unload()
    {

    }
}
