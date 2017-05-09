using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common;

namespace MrHai.Core.Models.Enums
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public class GlobalConstants
    {

        /// <summary>
        /// 主导航一级菜单名
        /// </summary>
        public static string[] MainNavigation = { "works", "news", "texts/publications", "artist cv", "2gg.studio", "contact", "links" };

        /// <summary>
        /// 作品导航一级菜单名称
        /// </summary>
        public static string[] ArticleNavigation = { "about", "film traller", "film stills", "installation", "exhibitions", "related texts", "publications" };

        /// <summary>
        /// 碎片
        /// </summary>
        public const string FragmentUrl = "/seainfo";

        /// <summary>
        /// 文章
        /// </summary>
        public const string TextsUrl = "/texts";

        /// <summary>
        /// 联系我们
        /// </summary>
        public const string ContactUsUrl = "/contactus";

        /// <summary>
        /// 作者简介
        /// </summary>
        public const string ArtistUrl = "/artist";

        /// <summary>
        /// 关于作品
        /// </summary>
        public const string ProaboutUrl = "/proabout";

        /// <summary>
        /// 视频
        /// </summary>
        public const string ProstillsUrl = "/protrailer";

        /// <summary>
        /// 视频截图
        /// </summary>
        public const string ProtrailerUrl = "/prostills";

        /// <summary>
        /// 安装过程
        /// </summary>
        public const string InstallationUrl = "/installation";

        /// <summary>
        /// 展览现场
        /// </summary>
        public const string ExhibitionsUrl = "/exhibitions";

        /// <summary>
        /// 相关文章
        /// </summary>
        public const string RelatedUrl = "/related";

        /// <summary>
        /// 评论
        /// </summary>
        public const string PublicationsUrl = "/pulications";


        public const string News = "/news";
    }
}

