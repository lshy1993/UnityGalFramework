using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

using Assets.Script.Framework.Data;

using UnityEngine;

//using LitJson;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 存读档工具
    /// </summary>
    public class SaveLoadTool
    {
        public static readonly string SAVE_PATH = Application.persistentDataPath + "/Save";

        /// <summary>
        /// 生成全路径
        /// </summary>
        public static string GetSavePath(string fileName)
        {
            return SAVE_PATH + "/" + fileName;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public static bool IsFileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        public static bool IsDirectoryExists(string fileName)
        {
            return Directory.Exists(fileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public static void DeleteFile(string fileName)
        {
            if (IsFileExists(fileName))
            {
                //如果存在则删除
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// 创建一个二进制文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="data">文件内容</param>
        public static void CreatByteFile(string fileName, byte[] data)
        {
            File.WriteAllBytes(fileName, data);
        }


        /// <summary>
        /// 创建一个文本文件    
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void CreateFile(string filePath, string content)
        {
            // 若存在则自动清空文件
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.Write(content);
            streamWriter.Close();

            //if (IsFileExists(fileName))
            //{
            //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write);
            //    fs.SetLength(0);
            //    StreamWriter streamWriter = File.CreateText(fileName);
            //    streamWriter.Write(content);
            //    streamWriter.Close();
            //}
            //else
            //{
            //    FileStream fs = new FileStream(fileName, FileMode.CreateNew);
            //    StreamWriter sw = new StreamWriter(fs);
            //    sw.Write(content);
            //    sw.Close();
            //}
        }

        /// <summary>
        /// 存储文件（含加密）
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <param name="content">文件内容</param>
        public static void SaveFile(string filePath, string content)
        {
            //开发模式下 明文存储
            string toSave = content;
            if (!Constants.DEBUG)
            {
                //加密
                toSave = RijndaelEncrypt(toSave, GetKey());
            }
            CreateDirectory(SAVE_PATH);
            CreateFile(filePath, toSave);
        }

        /// <summary>
        /// 读取文本文件（含解密）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static string LoadFile(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string toLoad = streamReader.ReadToEnd();
            if (!Constants.DEBUG)
            {
                //解密
                toLoad = RijndaelDecrypt(toLoad, GetKey());
            }
            streamReader.Close();
            return toLoad;
        }


        /// <summary>
        /// 创建一个文件夹
        /// </summary>
        public static void CreateDirectory(string fileName)
        {
            //文件夹存在则返回
            if (IsDirectoryExists(fileName))
                return;
            Directory.CreateDirectory(fileName);
        }

        /// <summary>
        /// 删除指定的文件夹
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteDirectory(string fileName)
        {
            //文件夹存在则删除
            if (IsDirectoryExists(fileName))
            {
                Directory.Delete(fileName);
            }
        }

        /// <summary>
        /// Rijndael加密算法
        /// </summary>
        /// <param name="pString">待加密的明文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        private static string RijndaelEncrypt(string pString, string pKey)
        {
            //密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
            //待加密明文数组
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            //返回加密后的密文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// ijndael解密算法
        /// </summary>
        /// <param name="pString">待解密的密文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        private static string RijndaelDecrypt(string pString, string pKey)
        {
            //解密密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
            //待解密密文数组
            byte[] toEncryptArray = Convert.FromBase64String(pString);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();

            //返回解密后的明文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 获取本台机器的加密种子
        /// </summary>
        /// <returns>种子串</returns>
        public static string GetKey()
        {
            string key = "";
            key = GetMacAddress();
            key += key;

            return key;
        }

        private static string GetMacAddress()
        {
            string physicalAddress = "";

            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adaper in nice)
            {

                //Debug.Log(adaper.Description);

                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();

                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }

            return physicalAddress;
        }

    }
}
