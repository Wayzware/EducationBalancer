namespace Wayz.CS2.SchoolCapacityBalancer;

public class SchoolCapacityBalancerOptions
{
    public static SchoolCapacityBalancerOptions Default => new SchoolCapacityBalancerOptions
    {
        Version = 1,
        Options = new List<SchoolOptions>
        {
            new SchoolOptions
            {
                Name = "ElementarySchool01",
                UpkeepCost = 50000,
                StudentCapacity = 2000
            },
            new SchoolOptions
            {
                Name = "ElementarySchool01 Extension Wing",
                UpkeepCost = 20000,
                StudentCapacity = 1000
            },
            new SchoolOptions
            {
                Name = "HighSchool01",
                UpkeepCost = -1,
                StudentCapacity = -1
            },
            new SchoolOptions
            {
                Name = "HighSchool01 Extension Wing",
                UpkeepCost = -1,
                StudentCapacity = -1
            },
            new SchoolOptions
            {
                Name = "College01",
                UpkeepCost = -1,
                StudentCapacity = -1
            },
            new SchoolOptions
            {
                Name = "College01 Extension Wing",
                UpkeepCost = 110000,
                StudentCapacity = 1000
            },
            new SchoolOptions
            {
                Name = "University01",
                UpkeepCost = 375000,
                StudentCapacity = 2500
            },
            new SchoolOptions
            {
                Name = "University01 Extension Wing",
                UpkeepCost = 125000,
                StudentCapacity = 1000
            }
        }
    };

    public int Version { get; set; }

    public IEnumerable<SchoolOptions> Options { get; set; } = new List<SchoolOptions>();

    public IReadOnlyDictionary<string, SchoolOptions> ToDictionary()
    {
        var dict = new Dictionary<string, SchoolOptions>();
        foreach (var option in Options)
        {
            dict.Add(option.Name, option);
        }
        return dict;
    }

    public int RemoveBadEntires()
    {
        var length = Options.Count();
        Options = Options.Where(x => !string.IsNullOrEmpty(x.Name) || (x.UpkeepCost < 0 && x.StudentCapacity < 0));
        return length - Options.Count();
    }

    /// <summary>
    /// Updates the settings to the latest version.
    /// </summary>
    /// <returns>The versions updated through</returns>
    public int UpdateToLatestVersion()
    {
        var initialVersion = Version;
        if(Version == 0)
        {
            string[] addedSchools = ["HighSchool01", "HighSchool01 Extension Wing", "College01", "TechnicalUniversity01", "TechnicalUniversity01 Extension Wing",  "MedicalUniversity01", "MedicalUniversity01 Extension Wing"];
            foreach(var school in addedSchools)
            {
                if (Options.Any(o => o.Name == school)) continue;

                Options = Options.Append(new SchoolOptions
                {
                    Name = school,
                    UpkeepCost = -1,
                    StudentCapacity = -1
                });
            }
            Options = Options.OrderBy(x => x.Name);
            Version = 1;
        }
        return Version - initialVersion;
    }
}