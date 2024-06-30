using Colossal.Serialization.Entities;
using Game;
using Game.Prefabs;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace SchoolCapacityBalancer;
public partial class SchoolCapacityChangerSystem : GameSystemBase
{
    private Dictionary<Entity, (SchoolData, ConsumptionData)> _schoolToData = new Dictionary<Entity, (SchoolData, ConsumptionData)>();

    private EntityQuery _query;

    protected override void OnCreate()
    {
        base.OnCreate();

        _query = GetEntityQuery(new EntityQueryDesc()
        {
            All = [
                ComponentType.ReadWrite<SchoolData>(),
                ComponentType.ReadWrite<ConsumptionData>()
            ]
        });

        RequireForUpdate(_query);
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
        base.OnGamePreload(purpose, mode);
        Enabled = true;
    }

    protected override void OnUpdate() 
    {
        var schools = _query.ToEntityArray(Allocator.Temp);

        foreach (var school in schools)
        {
            SchoolData schoolData;
            ConsumptionData serviceConsumption;

            if (_schoolToData.TryGetValue(school, out var data))
            {
                schoolData = data.Item1;
                serviceConsumption = data.Item2;
            }
            else
            {
                schoolData = EntityManager.GetComponentData<SchoolData>(school);
                serviceConsumption = EntityManager.GetComponentData<ConsumptionData>(school);

                _schoolToData.Add(school, (schoolData, serviceConsumption));
            }

            var educationLevel = schoolData.m_EducationLevel;
            double scalar = educationLevel switch
            {
                (byte)SchoolLevel.Elementary => Mod.m_Setting.ElementarySlider,
                (byte)SchoolLevel.HighSchool => Mod.m_Setting.HighSchoolSlider,
                (byte)SchoolLevel.College => Mod.m_Setting.CollegeSlider,
                (byte)SchoolLevel.University => Mod.m_Setting.UniversitySlider,
                _ => 0
            } / 100d;

            if (scalar == 0) continue;

            schoolData.m_StudentCapacity = (int)(scalar * schoolData.m_StudentCapacity);
            EntityManager.SetComponentData<SchoolData>(school, schoolData);

            if (Mod.m_Setting.ScaleUpkeepWithCapacity)
            {
                serviceConsumption.m_Upkeep = (int)(serviceConsumption.m_Upkeep * scalar);
            }
            EntityManager.SetComponentData<ConsumptionData>(school, serviceConsumption);
        }

        Enabled = false;
    }
}

