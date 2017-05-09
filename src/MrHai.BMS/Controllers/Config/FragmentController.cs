using MrHai.Application;
using MrHai.Application.Models;
using MrHai.BMS.Controllers.Base;
using MrHai.Common.GlobalConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers.Config
{
    //碎片管理
    [Export]
    public class FragmentController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        public int fragmentCount = 29;

        // GET: Fragment
        public ActionResult Index()
        {
            string fragmentCountStr = Configs.GetConfig("FragmentCount");

            fragmentCount = int.Parse(string.IsNullOrEmpty(fragmentCountStr) ? "29" : fragmentCountStr);

            var fragment = (MrHaiApplication.GetFragment ()?.AppendData) as Fragment ?? new Fragment();

            if (fragment.Fragments == null || fragment.Fragments.Count == 0)
            {//没有碎片时初始化
                fragment.Fragments = fragment.Fragments ?? new List<FragmentSetting>();

                for (int i = 1; i <= fragmentCount; i++)
                {
                    fragment.Fragments.Add(new FragmentSetting()
                    {
                        SerialNumber = i,
                        Enable = false,
                        WorksId = "-1"
                    });
                }
            }
            else
            {
                if (fragmentCount != fragment.Fragments.Count)
                {//碎片数量更改时更新
                    int count = fragment.Fragments.Count;
                    if (fragmentCount > count)
                    {
                        for (int i = 1; i <= (count - fragmentCount); i++)
                        {
                            fragment.Fragments.Add(new FragmentSetting()
                            {
                                SerialNumber = count + i,
                                Enable = false,
                                WorksId = "-1"
                            });
                        }
                    }
                    else
                    {
                        for (int i = fragmentCount - count; i > 0 ; i++)
                        {
                            fragment.Fragments.RemoveAt(i - 1);
                        }
                    }
                }
            }

            ViewBag.fragment = fragment;

            var works = (MrHaiApplication.GetWorksName()?.AppendData) as List<CategoryListDto>;
            ViewBag.Works = works ?? new List<CategoryListDto>();

            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            Fragment fragment = new Fragment();
            fragment.ID = Request["ID"];
            fragment.Fragments = new List<FragmentSetting>();

            for (int i = 1; i <= fragmentCount; i++)
            {
                fragment.Fragments.Add(new FragmentSetting()
                {
                    SerialNumber = i,
                    Enable = string.IsNullOrEmpty(Request["ckbEnable" + i]) ? false : true,
                    WorksId = Request["selWorksId" + i]
                });
            }

            MrHaiApplication.SaveFragment(fragment);

            return RedirectToAction("");
        }
    }
}