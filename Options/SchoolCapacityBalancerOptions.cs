using System.Collections.Generic;
using System.Linq;

namespace Wayz.CS2.SchoolCapacityBalancer;

public class SchoolCapacityBalancerOptions
{
    public IEnumerable<SchoolOptions> Options { get; set; } = new List<SchoolOptions>
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
    };

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
        Options = Options.Where(x => !string.IsNullOrEmpty(x.Name) && x.UpkeepCost >= 0 && x.StudentCapacity >= 0);
        return length - Options.Count();
    }
}