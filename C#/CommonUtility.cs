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
		#region getFileList �t�@�C���ꗗ�擾
		//*************************************************************************************
		/// <summary>
		/// �t�@�C���̈ꗗ���擾����B�i�T�u�f�B���N�g�����擾����j
		/// </summary>
		/// <param name="filePath">�ꗗ���擾����t�H���_�̃p�X</param>
		/// <param name="filePattern">�擾����t�@�C���̃p�^�[���i���K�\���j</param>
		/// <returns>�擾�����e�t�@�C���̃p�X</returns>
		//*************************************************************************************
		public static string[] getFileList(string filePath, string filePattern)
		{
			string[] fileList = Directory.GetFiles(filePath, filePattern, SearchOption.AllDirectories);
			return fileList;
		}
		#endregion

		#region replaceFileText �P��o�����擾
		//*************************************************************************************
		/// <summary>
		/// ������ɑ��݂���P��̏o�����𒲍�����B
		/// </summary>
		/// <param name="checkString">�����Ώە�����</param>
		/// <param name="checkWord">�o�����J�E���g�Ώە���</param>
		/// <returns>�Ώە����̏o����</returns>
		//*************************************************************************************
		public static int countWordInString(string checkString, string checkWord)
		{
			if (checkWord == null || checkWord == "")
			{
				throw new Exception("�u���Ώە�����null�������͋󕶎������e���܂���B");
			}

			int value = checkString.Length - checkString.Replace(checkWord, "").Length;
			if (value != 0)
			{
				value = value / checkWord.Length;
			}
			return value;
		}
		#endregion

		#region replaceFileText �t�@�C���u���o��
		//*************************************************************************************
		/// <summary>
		/// �t�@�C���̕������u�����ďo�͂���B
		/// </summary>
		/// <param name="inFilePath">�u���Ώۃt�@�C���p�X</param>
		/// <param name="outFilePath">�o�̓t�@�C���p�X</param>
		/// <param name="fileEncode">�����R�[�h</param>
		/// <param name="replacePair">�u��������KeyValue</param>
		/// <returns>�Ȃ��i�t�@�C���o�́j</returns>
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

		#region renameFile �t�@�C�����ύX�i�t�@�C���̈ړ����\�j
		//*************************************************************************************
		/// <summary>
		/// �t�@�C�����ύX
		/// </summary>
		/// <param name="beforeFilePath">�ύX�O�t�@�C���p�X</param>
		/// <param name="afterFilePath">�ύX��t�@�C���p�X</param>
		/// <returns>�Ȃ�</returns>
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

		#region deleteFile �t�@�C���폜
		//*************************************************************************************
		/// <summary>
		/// �t�@�C���폜
		/// </summary>
		/// <param name="FilePath">�폜�Ώۂ̃t�@�C���p�X</param>
		/// <returns>�Ȃ�</returns>
		//*************************************************************************************
		public static void deleteFile(string FilePath)
		{
			File.Delete(FilePath);
		}
		#endregion

		#region splitString �������؂�i�P��f���~�^�j
		//*************************************************************************************
		/// <summary>
		/// ��������w��̃f���~�^�ŋ�؂��ĕԋp����B
		/// ��string��string�ŋ�؂�֐����Ȃ����ߍ쐬
		/// </summary>
		/// <param name="tagetStr">��؂�Ώە�����</param>
		/// <param name="delimiter">��؂蕶����</param>
		/// <returns>��؂����������z��ŕԂ��B</returns>
		//*************************************************************************************
		public static string[] splitString(string tagetStr, string delimiter)
		{
			string[] delimiters = { delimiter };
			string[] parts = tagetStr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			return parts;
		}
		#endregion

		#region splitString �������؂�i�����f���~�^�j
		//*************************************************************************************
		/// <summary>
		/// ������𕡐��̃f���~�^�ŋ�؂��ĕԋp����B
		/// ��string��string�ŋ�؂�֐����Ȃ����ߍ쐬
		/// </summary>
		/// <param name="tagetStr">��؂�Ώە�����</param>
		/// <param name="delimiter">��؂蕶���z��</param>
		/// <returns>��؂����������z��ŕԂ��B</returns>
		//*************************************************************************************
		public static string[] splitString(string tagetStr, string[] delimiters)
		{
			string[] parts = tagetStr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			return parts;
		}
		#endregion

		#region splitStringRegex �������؂�i���K�\���f���~�^�j
		//*************************************************************************************
		/// <summary>
		/// ������𐳋K�\���̃f���~�^�ŋ�؂��ĕԋp����B
		/// ���Ƃ��΁u"\"\\s*,\\s*\""(",")�v�Ƃ�
		/// </summary>
		/// <param name="tagetStr">��؂�Ώە�����</param>
		/// <param name="delimiter">���K�\���̋�؂蕶����</param>
		/// <returns>��؂����������z��ŕԂ��B</returns>
		//*************************************************************************************
		public static string[] splitStringRegex(string tagetStr, string delimiter)
		{
			return Regex.Split(tagetStr, delimiter);
		}
		#endregion

		#region getPositionsOfWord �P��̈ʒu����
		//*************************************************************************************
		/// <summary>
		/// ������ɏo������P��̈ʒu�𒲂ׂ�B�i��������ꍇ�͂��̑S�Ă�Ԃ��B�j
		/// </summary>
		/// <param name="tagetStr">�����Ώە�����</param>
		/// <param name="targetWord">�����ΏےP��</param>
		/// <returns>�P��o���ʒu�̔z��i�Ȃ��ꍇ��null�j</returns>
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

		#region replaceStringByPosition �ʒu�ɂ�镶���u��
		//*************************************************************************************
		/// <summary>
		/// �Ώۂ̕����񂩂�w�肳�ꂽ�ʒu�n�܂�̎w�肳�ꂽ�����̕�����u������B
		/// </summary>
		/// <param name="targetStr">�u���Ώە�����</param>
		/// <param name="startPos">�u���J�n�ʒu</param>
		/// <param name="posLength">�u����������</param>
		/// <param name="replaceWord">�u�������P��</param>
		/// <returns>�u����̕����񂪕ԋp�����B</returns>
		//*************************************************************************************
		public static string replaceStringByPosition(string targetStr, int startPos, int posLength, string replaceWord)
		{
			targetStr = targetStr.Remove(startPos, posLength);
			targetStr = targetStr.Insert(startPos, replaceWord);
			return targetStr;
		}
		#endregion

		#region getCurrentDirectoryPath �J�����g�f�B���N�g���p�X�擾
		//*************************************************************************************
		/// <summary>
		/// �o�b�`�����s����Ă���t�H���_�̃p�X���擾����B
		/// </summary>
		/// <returns>�J�����g�f�B���N�g���p�X</returns>
		//*************************************************************************************
		public static string getCurrentDirectoryPath()
		{
			return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		}
		#endregion

		#region getExecutionFileName ���s�t�@�C�����擾
		//*************************************************************************************
		/// <summary>
		/// ���s���Ă���o�b�`�̃t�@�C�������g���q�����Ŏ擾����B
		/// </summary>
		/// <returns>�g���q�����̃t�@�C����</returns>
		//*************************************************************************************
		public static string getExecutionFileName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
		}
		#endregion

		#region setLeafNode ���[�m�[�h�擾
		//*************************************************************************************
		/// <summary>
		/// �w�肳�ꂽXML�̖��[�m�[�h�̗v�f���ƒl��Dictionary�ɒǉ�����B
		/// ���ċN�ɂ���đS�Ă̖��[�m�[�h�𒲍����Ă���B
		/// </summary>
		/// <param name="parentNodeList">���[�g�m�[�h��NodeList</param>
		/// <param name="ref dict">�v�f���E�l��ǉ����Ă���Dictionary�iref���w�肷�邱�Ɓj</param>
		/// <returns>�Ȃ��i�Q�Ɠn�����ꂽDictionary�ɑS�ēo�^����Ă���j</returns>
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

		#region �L�[�\�[�g�i�~���j�p�֐�
		//*************************************************************************************
		/// <summary>
		/// List�^�̃\�[�g���s�����߂̊֐�
		/// ������̓L�[�̍~���Ń\�[�g����B
		/// 
		/// �ȉ��̂悤�ɌĂяo�������ł����B
		/// List<KeyValuePair<string, string>>.Sort(Utility.CompareKeyValuePair);
		/// ���g���E���ς���΃\�[�g���@�͖�����
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		//*************************************************************************************
		public static int CompareKeyValuePair(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
		{
			return y.Key.Length - x.Key.Length;
		}
		#endregion

		#region MD5�ϊ�
		//*************************************************************************************
		/// <summary>
		/// �����̕�������n�b�V�����iMD5�j����B
		/// </summary>
		/// <param name="str">�ϊ�������������</param>
		/// <returns>�n�b�V���l�iMD5�j</returns>
		//*************************************************************************************
		public static string convertStringToMD5(string str)
		{
			byte[] bs = null;
			byte[] data = null;
			StringBuilder result = new StringBuilder();

			data = Encoding.UTF8.GetBytes(str);

			// �n�b�V���l���v�Z����
			using (MD5CryptoServiceProvider algorithm = new MD5CryptoServiceProvider())
			{
				bs = algorithm.ComputeHash(data);
			}

			// �o�C�g�^�z���16�i��������ɕϊ�
			foreach (byte b in bs)
			{
				result.Append(b.ToString("X2"));
			}

			return result.ToString();
		}
		#endregion

		#region getCSV CSV�擾
		//*************************************************************************************
		/// <summary>
		/// CSV�t�@�C����Ǎ����AArrayList��String[]�̓񎟌��z��ɂ��ĕԂ��B
		/// </summary>
		/// <param name="FilePath">CSV�t�@�C���̃p�X</param>
		/// <param name="strEncoding">CSV�t�@�C���̃G���R�[�h�����i�Ȃ��Ă������j</param>
		/// <returns>CSV��ǂݍ���ArrayList</returns>
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

			//��؂蕶����,�Ƃ���
			tfp.Delimiters = new string[] { "," };
			//�t�B�[���h��"�ň͂݁A���s�����A��؂蕶�����܂߂邱�Ƃ��ł��邩
			//�f�t�H���g��true�Ȃ̂ŁA�K�v�Ȃ�
			tfp.HasFieldsEnclosedInQuotes = true;
			//�t�B�[���h�̑O�ォ��X�y�[�X���폜����
			//�f�t�H���g��true�Ȃ̂ŁA�K�v�Ȃ�
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

		#region fileReplace �t�@�C��������u��
		//*************************************************************************************
		/// <summary>
		/// �w�肳�ꂽ�t�@�C���ɑ΂��ĕ�����u�����s���B
		/// </summary>
		/// <param name="FilePath">�u�����s���t�@�C���̃p�X</param>
		/// <param name="strPair">�u���O������ƒu���㕶����̃y�A�̃��X�g</param>
		/// <param name="strEncoding">CSV�t�@�C���̃G���R�[�h�����i�Ȃ��Ă������j</param>
		/// <returns>CSV��ǂݍ���ArrayList</returns>
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
