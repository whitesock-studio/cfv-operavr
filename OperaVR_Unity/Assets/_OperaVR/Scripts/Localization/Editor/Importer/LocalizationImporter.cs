using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SpatialSys.UnitySDK.SpatialNetworkVariables;

namespace OperaVR
{
#if UNITY_EDITOR
    public class LocalizationImporter : MonoBehaviour
    {
        [SerializeField]
        private string _sheetId;

        [SerializeField]
        private LocalizationData _localizationData;

        private void Start()
        {
            Import();
        }

        public void Import()
        {
            StartCoroutine(CSVDownloader.DownloadData(PostProcessData, _sheetId));
        }

        private void PostProcessData(string data)
        {
            _localizationData = new LocalizationData();

            var result = CSVToLocalizationData.ParseCsv(data);
            var languagesRow = result[0];
            var languagesKeys = new List<string>();
            for (var i = 1; i < languagesRow.Length; i++)
            {
                languagesKeys.Add(languagesRow[i]);
            }

            _localizationData.InitalizeLanguageTables(languagesKeys);

            result.RemoveAt(0);
            foreach (var rowData in result)
            {
                var key = "";
                for (var i = 0; i < rowData.Length; i++)
                {
                    if (i == 0)
                    {
                        key = rowData[i];
                        continue;
                    }
                    _localizationData.InsertValue(languagesKeys[i - 1], key, rowData[i]);
                }
            }
        }
    }
#endif
}
