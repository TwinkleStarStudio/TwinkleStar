using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Core.Models
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
        BackgroundImage = 7,
        [Remark("关于作品")]
        About = 8,
        [Remark("摘要")]
        Summary = 9,
        [Remark("主导航文章")]
        MainArtistCV=10,
        [Remark("最新资讯")]
        News=11,

        [Remark("其他")]
        Other=999
    }
    /// <summary>
    /// 父节点类型
    /// </summary>
    public enum NodeType
    {

        [Remark("菜单")]
        Category = 0,
        [Remark("Work")]
        Work = 1
    }
}
