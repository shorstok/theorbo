using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace theorbo.MusicTheory
{
    public class IanringSource
    {
        public IanringSource()
        {
            IanringScales =
                JsonConvert.DeserializeObject<Dictionary<int, IanringScaleEntity>>(
                    Encoding.UTF8.GetString(Resources.ianring_scales));

            IanringScalesByStepsCyclic = new Dictionary<string, IanringScaleEntity>();

            foreach (var scale in IanringScales)
            {
                var key = string.Join(string.Empty, ScaleHelper.GetHalftoneStepsFromBitmask(scale.Key));

                IanringScalesByStepsCyclic[key] =
                    scale.Value;
            }
        }

        public Dictionary<int, IanringScaleEntity> IanringScales { get; }
        public Dictionary<string, IanringScaleEntity> IanringScalesByStepsCyclic { get; }
    }
}