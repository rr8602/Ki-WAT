using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{

	public static class DictionaryExtensions
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
		{
			if (dict == null) return defaultValue;
			return dict.TryGetValue(key, out var value) ? value : defaultValue;
		}
	}
	public class JsonController
	{
		public void SaveLangFile(string fileName, LangController langCtrl)
		{
			string workPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Lang") ;
			
			if (!Directory.Exists(workPath))
				Directory.CreateDirectory(workPath);

			string filePath = Path.Combine(workPath, fileName);
			string json = JsonConvert.SerializeObject(langCtrl, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		public LangController LoadLangFile(string fileName)
		{
			string workPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Lang");
			string filePath = Path.Combine(workPath, fileName);

			if (!File.Exists(filePath))
				return new LangController(); // 파일 없으면 기본값 반환

			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<LangController>(json);
		}

		public void EnsureLangFileExists(string fileName, LangController langCtrl)
		{
			string workPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Lang");
			

			// 폴더 없으면 생성
			if (!Directory.Exists(workPath))
				Directory.CreateDirectory(workPath);

			string filePath = Path.Combine(workPath, fileName);

			// 파일 없으면 SaveLangFile 호출
			if (!File.Exists(filePath))
			{
				SaveLangFile(fileName, langCtrl);
				//SaveLangDataToJson(langCtrl, filePath);
				MessageBox.Show("Lang 파일이 없어서 새로 만들었습니다: " + filePath);
			}
		}


	}
}
