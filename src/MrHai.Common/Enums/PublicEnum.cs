using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Common
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum NavigationType
    {
        [Remark("主导航")]
        MainNavigation = 0,
        [Remark("作品导航")]
        ArticleNavigation = 1
    }

    /// <summary>
    /// 资讯类型
    /// </summary>
    public enum InfomationType
    {
        [Remark("视频")]
        Video = 0,
        [Remark("文章")]
        Article = 1,
        [Remark("评论")]
        Comments = 2,
        [Remark("作者简介")]
        ArtistCV = 3,
        [Remark("展览现场")]
        Exhibitions = 4,
        [Remark("组装过程")]
        Installation = 5,
        [Remark("视频截图")]
        FilmStill = 6,
        [Remark("背景图片")]
        BackgroundImage = 7
    }
}
