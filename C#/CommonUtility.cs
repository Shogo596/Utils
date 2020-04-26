using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml;
using System.Security.Cryptography;

namespace CommonUtility
{
	public class Utility
	{
		#region getFileList ファイル一覧取得
		//*************************************************************************************
		/// <summary>
		/// ファイルの一覧を取得する。（サブディレクトリも取得する）
		/// </summary>
		/// <param name="filePath">一覧を取得するフォルダのパス</param>
		/// <param name="filePattern">取得するファイルのパターン（正規表現）</param>
		/// <returns>取得した各ファイルのパス</returns>
		//*************************************************************************************
		public static string[] getFileList(string filePath, string filePattern)
		{
			string[] fileList = Directory.GetFiles(filePath, filePattern, SearchOption.AllDirectories);
			return fileList;
		}
		#endregion

		#region replaceFileText 単語出現数取得
		//*************************************************************************************
		/// <summary>
		/// 文字列に存在する単語の出現数を調査する。
		/// </summary>
		/// <param name="checkString">調査対象文字列</param>
		/// <param name="checkWord">出現数カウント対象文字</param>
		/// <returns>対象文字の出現数</returns>
		//*************************************************************************************
		public static int countWordInString(string checkString, string checkWord)
		{
			if (checkWord == null || checkWord == "")
			{
				throw new Exception("置換対象文字はnullもしくは空文字を許容しません。");
			}

			int value = checkString.Length - checkString.Replace(checkWord, "").Length;
			if (value != 0)
			{
				value = value / checkWord.Length;
			}
			return value;
		}
		#endregion

		#region replaceFileText ファイル置換出力
		//*************************************************************************************
		/// <summary>
		/// ファイルの文字列を置換して出力する。
		/// </summary>
		/// <param name="inFilePath">置換対象ファイルパス</param>
		/// <param name="outFilePath">出力ファイルパス</param>
		/// <param name="fileEncode">文字コード</param>
		/// <param name="replacePair">置換文字のKeyValue</param>
		/// <returns>なし（ファイル出力）</returns>
		//*************************************************************************************
		public static void replaceFileText(string inFilePath, string outFilePath, string fileEncode, KeyValuePair<string, string> replacePair)
		{
			string lineTxt = "";

			using (StreamReader stReader = new StreamReader(inFilePath, Encoding.GetEncoding(fileEncode)))
			{
				using (StreamWriter stWriter = new StreamWriter(outFilePath, false, Encoding.GetEncoding(fileEncode)))
				{
					while (stReader.Peek() >= 0)
					{
						lineTxt = stReader.ReadLine();

						if (lineTxt.IndexOf(replacePair.Key) >= 0)
						{
							lineTxt = lineTxt.Replace(replacePair.Key, replacePair.Value);
						}

						stWriter.WriteLine(lineTxt);
					}
				}
			}
		}
		#endregion

		#region renameFile ファイル名変更（ファイルの移動も可能）
		//*************************************************************************************
		/// <summary>
		/// ファイル名変更
		/// </summary>
		/// <param name="beforeFilePath">変更前ファイルパス</param>
		/// <param name="afterFilePath">変更後ファイルパス</param>
		/// <returns>なし</returns>
		//*************************************************************************************
		public static void renameFile(string beforeFilePath, string afterFilePath)
		{
			if (File.Exists(afterFilePath))
			{
				File.Delete(afterFilePath);
			}
			File.Move(beforeFilePath, afterFilePath);
		}
		#endregion

		#region deleteFile ファイル削除
		//*************************************************************************************
		/// <summary>
		/// ファイル削除
		/// </summary>
		/// <param name="FilePath">削除対象のファイルパス</param>
		/// <returns>なし</returns>
		//*************************************************************************************
		public static void deleteFile(string FilePath)
		{
			File.Delete(FilePath);
		}
		#endregion

		#region splitString 文字列区切り（単一デリミタ）
		//*************************************************************************************
		/// <summary>
		/// 文字列を指定のデリミタで区切って返却する。
		/// ※stringをstringで区切る関数がないため作成
		/// </summary>
		/// <param name="tagetStr">区切り対象文字列</param>
		/// <param name="delimiter">区切り文字列</param>
		/// <returns>区切った文字列を配列で返す。</returns>
		//*************************************************************************************
		public static string[] splitString(string tagetStr, string delimiter)
		{
			string[] delimiters = { delimiter };
			string[] parts = tagetStr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			return parts;
		}
		#endregion

		#region splitString 文字列区切り（複数デリミタ）
		//*************************************************************************************
		/// <summary>
		/// 文字列を複数のデリミタで区切って返却する。
		/// ※stringをstringで区切る関数がないため作成
		/// </summary>
		/// <param name="tagetStr">区切り対象文字列</param>
		/// <param name="delimiter">区切り文字配列</param>
		/// <returns>区切った文字列を配列で返す。</returns>
		//*************************************************************************************
		public static string[] splitString(string tagetStr, string[] delimiters)
		{
			string[] parts = tagetStr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			return parts;
		}
		#endregion

		#region splitStringRegex 文字列区切り（正規表現デリミタ）
		//*************************************************************************************
		/// <summary>
		/// 文字列を正規表現のデリミタで区切って返却する。
		/// たとえば「"\"\\s*,\\s*\""(",")」とか
		/// </summary>
		/// <param name="tagetStr">区切り対象文字列</param>
		/// <param name="delimiter">正規表現の区切り文字列</param>
		/// <returns>区切った文字列を配列で返す。</returns>
		//*************************************************************************************
		public static string[] splitStringRegex(string tagetStr, string delimiter)
		{
			return Regex.Split(tagetStr, delimiter);
		}
		#endregion

		#region getPositionsOfWord 単語の位置検索
		//*************************************************************************************
		/// <summary>
		/// 文字列に出現する単語の位置を調べる。（複数ある場合はその全てを返す。）
		/// </summary>
		/// <param name="tagetStr">調査対象文字列</param>
		/// <param name="targetWord">調査対象単語</param>
		/// <returns>単語出現位置の配列（ない場合はnull）</returns>
		//*************************************************************************************
		public static int[] getPositionsOfWord(string tagetStr, string targetWord)
		{
			int count = countWordInString(tagetStr, targetWord);
			if (count == 0)
			{
				return null;
			}

			int[] positions = new int[count];

			int position = 0;
			for (int i = 0; i < positions.Length; i++)
			{
				position = tagetStr.IndexOf(targetWord, position);
				positions[i] = position;
				position++;
			}

			return positions;
		}
		#endregion

		#region replaceStringByPosition 位置による文字置換
		//*************************************************************************************
		/// <summary>
		/// 対象の文字列から指定された位置始まりの指定された長さの文字を置換する。
		/// </summary>
		/// <param name="targetStr">置換対象文字列</param>
		/// <param name="startPos">置換開始位置</param>
		/// <param name="posLength">置換文字長さ</param>
		/// <param name="replaceWord">置き換え単語</param>
		/// <returns>置換後の文字列が返却される。</returns>
		//*************************************************************************************
		public static string replaceStringByPosition(string targetStr, int startPos, int posLength, string replaceWord)
		{
			targetStr = targetStr.Remove(startPos, posLength);
			targetStr = targetStr.Insert(startPos, replaceWord);
			return targetStr;
		}
		#endregion

		#region getCurrentDirectoryPath カレントディレクトリパス取得
		//*************************************************************************************
		/// <summary>
		/// バッチが実行されているフォルダのパスを取得する。
		/// </summary>
		/// <returns>カレントディレクトリパス</returns>
		//*************************************************************************************
		public static string getCurrentDirectoryPath()
		{
			return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		}
		#endregion

		#region getExecutionFileName 実行ファイル名取得
		//*************************************************************************************
		/// <summary>
		/// 実行しているバッチのファイル名を拡張子抜きで取得する。
		/// </summary>
		/// <returns>拡張子抜きのファイル名</returns>
		//*************************************************************************************
		public static string getExecutionFileName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
		}
		#endregion

		#region setLeafNode 末端ノード取得
		//*************************************************************************************
		/// <summary>
		/// 指定されたXMLの末端ノードの要素名と値をDictionaryに追加する。
		/// ※再起によって全ての末端ノードを調査している。
		/// </summary>
		/// <param name="parentNodeList">ルートノードのNodeList</param>
		/// <param name="ref dict">要素名・値を追加していくDictionary（refを指定すること）</param>
		/// <returns>なし（参照渡しされたDictionaryに全て登録されている）</returns>
		//*************************************************************************************
		public static void setLeafNode(XmlNodeList parentNodeList, ref Dictionary<string, string> dict)
		{
			string key = string.Empty;
			string value = string.Empty;

			foreach (XmlNode node in parentNodeList)
			{
				if (node.NodeType == XmlNodeType.Text)
				{
					dict.Add(node.ParentNode.LocalName, node.ParentNode.InnerText);
				}
				else
				{
					setLeafNode(node.ChildNodes, ref dict);
				}
			}
		}
		#endregion

		#region キーソート（降順）用関数
		//*************************************************************************************
		/// <summary>
		/// List型のソートを行うための関数
		/// ※これはキーの降順でソートする。
		/// 
		/// 以下のように呼び出すだけでいい。
		/// List<KeyValuePair<string, string>>.Sort(Utility.CompareKeyValuePair);
		/// ※拡張・改変すればソート方法は無限大
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		//*************************************************************************************
		public static int CompareKeyValuePair(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
		{
			return y.Key.Length - x.Key.Length;
		}
		#endregion

		#region MD5変換
		//*************************************************************************************
		/// <summary>
		/// 引数の文字列をハッシュ化（MD5）する。
		/// </summary>
		/// <param name="str">変換したい文字列</param>
		/// <returns>ハッシュ値（MD5）</returns>
		//*************************************************************************************
		public static string convertStringToMD5(string str)
		{
			byte[] bs = null;
			byte[] data = null;
			StringBuilder result = new StringBuilder();

			data = Encoding.UTF8.GetBytes(str);

			// ハッシュ値を計算する
			using (MD5CryptoServiceProvider algorithm = new MD5CryptoServiceProvider())
			{
				bs = algorithm.ComputeHash(data);
			}

			// バイト型配列を16進数文字列に変換
			foreach (byte b in bs)
			{
				result.Append(b.ToString("X2"));
			}

			return result.ToString();
		}
		#endregion

		#region getCSV CSV取得
		//*************************************************************************************
		/// <summary>
		/// CSVファイルを読込し、ArrayListとString[]の二次元配列にして返す。
		/// </summary>
		/// <param name="FilePath">CSVファイルのパス</param>
		/// <param name="strEncoding">CSVファイルのエンコード文字（なくてもいい）</param>
		/// <returns>CSVを読み込んだArrayList</returns>
		//*************************************************************************************
		public static ArrayList getCSV(string FilePath)
		{
			return getCSV(FilePath, "");
		}
	
		public static ArrayList getCSV(string FilePath, string strEncoding)
		{
			ArrayList csvData = new ArrayList();

			if (strEncoding == "")
			{
				strEncoding = "Shift_JIS";
			}

			Encoding encode = Encoding.GetEncoding(strEncoding);

			Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(FilePath, encode);

			//区切り文字を,とする
			tfp.Delimiters = new string[] { "," };
			//フィールドを"で囲み、改行文字、区切り文字を含めることができるか
			//デフォルトでtrueなので、必要なし
			tfp.HasFieldsEnclosedInQuotes = true;
			//フィールドの前後からスペースを削除する
			//デフォルトでtrueなので、必要なし
			tfp.TrimWhiteSpace = true;

			while (!tfp.EndOfData)
			{
				string[] fields = tfp.ReadFields();
				csvData.Add(fields);
			}

			tfp.Close();

			return csvData;
		}
		#endregion

		#region fileReplace ファイル文字列置換
		//*************************************************************************************
		/// <summary>
		/// 指定されたファイルに対して文字列置換を行う。
		/// </summary>
		/// <param name="FilePath">置換を行うファイルのパス</param>
		/// <param name="strPair">置換前文字列と置換後文字列のペアのリスト</param>
		/// <param name="strEncoding">CSVファイルのエンコード文字（なくてもいい）</param>
		/// <returns>CSVを読み込んだArrayList</returns>
		//*************************************************************************************
		public void fileReplace(string FilePath, SortedList<string, string> strPair)
		{
			fileReplace(FilePath,strPair,"");
		}
	
		public void fileReplace(string FilePath, SortedList<string, string> strPair, string strEncoding)
		{
			string TempFilePath = FilePath + ".temp";

			if (strEncoding == "")
			{
				strEncoding = "Shift_JIS";
			}

			Encoding encode = Encoding.GetEncoding(strEncoding);
			StringBuilder sb = new StringBuilder();

			using (StreamReader sr = new StreamReader(FilePath, encode))
			using (StreamWriter sw = new StreamWriter(TempFilePath, false, encode))
			{
				try
				{
					string strBuffer;
					while (sr.Peek() >= 0)
					{
						strBuffer = sr.ReadLine();
						foreach (KeyValuePair<string, string> pair in strPair)
						{
							if (strBuffer.IndexOf(pair.Key) >= 0)
							{
								strBuffer = strBuffer.Replace(pair.Key, pair.Value);
							}
						}
						sw.WriteLine(strBuffer);
					}
				}
				catch
				{
					throw;
				}

				sr.Close();
				sw.Close();

				deleteFile(FilePath);
				renameFile(TempFilePath, FilePath);
			}
		}


		#endregion
	}
}
