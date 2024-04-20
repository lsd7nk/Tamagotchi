using Application = Tamagotchi.Application;
using System.Collections.Generic;
using Modules.Navigation;
using Settings.Job;
using UnityEngine;
using System.Linq;
using Core.Job;
using Settings;

namespace UI.Controller.Screen
{
    public sealed class JobScreenController : ScreenController
    {
        [Header("Controller")]
        [SerializeField] private RectTransform _layoutsParent;

        [Space]
        [SerializeField] private JobButtonController _buttonPrefab;
        [SerializeField] private RectTransform _layoutPrefab;

        private NavigationBlock _navigationBlock;
        private NavigationPoint _navigationPoint;

        private JobSettings _settings;

        public override void Setup()
        {
            base.Setup();

            if (_navigationBlock == null || _navigationPoint == null)
                return;

            var jobList = Application.Model.GetAvailableJob().ToArray();

            if (jobList == null)
                return;

            var layoutRectList = new List<RectTransform>();
            var layoutRectIdx = 0;

            for (var i = 0; i < jobList.Length; ++i)
            {
                if (layoutRectList.Count - 1 < layoutRectIdx)
                    layoutRectList.Add(Instantiate(_layoutPrefab, _layoutsParent));

                var icon = (Sprite)default;
                var job = jobList[i];

                var title = string.Empty;
                var content = string.Empty;

                if (job is FullTimeJob fullTimeJob)
                {
                    var settings = _settings.GetFullTimeJobSettings(fullTimeJob.JobType);

                    icon = settings.Icon;
                    title = _settings.Localization.GetFulltimeJobName(fullTimeJob.JobType);

                    for (int j = 0; j < settings.WorkingHours.Count; ++j)
                    {
                        content += (j == settings.WorkingHours.Count - 1)
                            ? $"{settings.WorkingHours[j]} {_settings.Localization.HoursText}"
                            : $"{settings.WorkingHours[j]}, ";
                    }
                }
                else if (job is PartTimeJob partTimeJob)
                {
                    icon = _settings.GetPartTimeJobSettings(partTimeJob.JobType).Icon;
                    title = _settings.Localization.GetParttimeJobName(partTimeJob.JobType);
                }

                Instantiate(_buttonPrefab, layoutRectList[layoutRectIdx]).Setup(job, icon, title, content);

                if (i % 2 != 0)
                    ++layoutRectIdx;
            }
        }

        private void Awake()
        {
            _navigationBlock = Application.Model.GetCurrentNavigationBlock();
            _navigationPoint = Application.Model.GetCurrentNavigationPoint();

            _settings = SettingsProvider.Get<JobSettings>();
        }
    }
}