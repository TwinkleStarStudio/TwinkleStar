using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MrHai.BMS.Models.Work
{
    public class WorkViewModel
    {

        public string infoID { set; get; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string ca { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { set; get; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string caName { set; get; }
        /// <summary>
        /// 作品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public string backgrand { get; set; }
        
        /// <summary>
        /// 视频
        /// </summary>
        public string video { get; set; }
        /// <summary>
        /// 视频截图
        /// </summary>
        public string screenShot { get; set; }

        public string[] screenShotArray
        {
            get
            {
                return screenShot?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            }
        }
        /// <summary>
        /// 组装过程
        /// </summary>
        public string assembly { get; set; }


        public string [] assemblyArray
        {
            get
            {
                return assembly?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            }
        }
        /// <summary>
        /// 关于作品
        /// </summary>
        public string about { get; set; }
        public string summary { set; get; }

        public DateTime? createTime { set; get; }



        public string content { set; get; }

        public string subContent
        {
            get
            {
                if (content != null && content.Length > 100)
                {
                    return content.Substring(0, 100);
                }
                else
                {
                    return content;
                }
            }
        }
    }


    public class viewAddModel
    {
        public string infoID { set; get; }
        public string workId { set; get; }
        public string title { set; get; }
        public string workName { set; get; }
        public string content { set; get; }

        

        public string [] showImage
        {
            get
            {
                return content?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)??new string[0];
            }
        }
        public int? userID { set; get; }
        public string cdID { set; get; }
        public InfomationType infoType { set; get; }
    }
}