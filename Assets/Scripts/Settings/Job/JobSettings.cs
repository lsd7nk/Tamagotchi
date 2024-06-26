using System.Collections.Generic;
using Modules.Localization;
using System.Linq;
using UnityEngine;
using Core.Job;
using System;

namespace Settings.Job
{
    [CreateAssetMenu(fileName = "JobSettings", menuName = "Settings/Job/JobSettings", order = 0)]
    public sealed class JobSettings : ScriptableObject
    {
        [field: SerializeField] public JobLocalization Localization { get; private set; }
        [field: SerializeField] public List<JobTypeSettings> JobTypeSettings { get; private set; }
        [field: Space]
        [field: SerializeField][field: Range(3, 6)] public int PartTimeAmountPerDay { get; private set; }
        [field: SerializeField][field: Range(1, 24)] public int PartTimeRecoveryHours { get; private set; }

        public FullTimeJobSettings GetFullTimeJobSettings(FullTimeJobType type)
        {
            var fullTimeJobSettings = JobTypeSettings.Where(s => s is FullTimeJobSettings).Select(s => s as FullTimeJobSettings);
            var settings = fullTimeJobSettings.FirstOrDefault(s => s.JobType == type);

            return settings;
        }

        public PartTimeJobSettings GetPartTimeJobSettings(PartTimeJobType type)
        {
            var fullTimeJobSettings = JobTypeSettings.Where(s => s is PartTimeJobSettings).Select(s => s as PartTimeJobSettings);
            var settings = fullTimeJobSettings.FirstOrDefault(s => s.JobType == type);

            return settings;
        }

#if UNITY_EDITOR
        public void Add(JobTypeSettings settings)
        {
            if (JobTypeSettings.Contains(settings))
                return;

            JobTypeSettings.Add(settings);
        }
#endif


        [Serializable]
        public sealed class JobLocalization
        {
            public string HoursText => LocalizationProvider.GetText(_asset, "description/hours");

            [SerializeField] private LocalizedText _asset;

            public string GetFulltimeJobName(FullTimeJobType type)
            {
                return LocalizationProvider.GetText(_asset, $"title/full_time/{type}");
            }

            public string GetPartTimeJobName(PartTimeJobType type)
            {
                return LocalizationProvider.GetText(_asset, $"title/part_time/{type}");
            }
        }
    }
}