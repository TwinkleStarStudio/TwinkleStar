using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System;

namespace TwinkleStar.Common.Excel
{
    public class ExcelHelper
    {
        /// <summary>
        /// excel转换成实体
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, ExcelEntity> ExcelToExcelEntity(string filePath)
        {
            Dictionary<string, ExcelEntity> excelEntity = new Dictionary<string, ExcelEntity>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {

                IWorkbook workbook = null;
                bool isO3Excel = false;
                if (filePath.Last() == 's')
                {
                    workbook = new HSSFWorkbook(fs);
                    isO3Excel = true;
                }
                else
                {
                    workbook = new XSSFWorkbook(fs);
                    isO3Excel = false;
                }

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    string sheetName = sheet.SheetName;
                    excelEntity.Add(sheetName, new ExcelEntity());

                    //获取表格信息
                    for (int rowIndex = sheet.FirstRowNum; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        for (int colIndex = sheet.GetRow(rowIndex).FirstCellNum; colIndex <= sheet.GetRow(rowIndex).LastCellNum; colIndex++)
                        {
                            if (excelEntity[sheetName].CellEntities == null)
                            {
                                excelEntity[sheetName].CellEntities = new List<CellEntity>();
                            }

                            CellEntity cell = new CellEntity();
                            ICell excelCell = null;
                            if (isO3Excel)
                            {
                                excelCell =  sheet.GetRow(rowIndex).GetCell(colIndex) as HSSFCell;
                            }
                            else
                            {
                                excelCell = sheet.GetRow(rowIndex).GetCell(colIndex) as XSSFCell;
                            }
                            cell.Cell = excelCell;
                            cell.FirstRow = rowIndex;
                            cell.FirstCol = colIndex;
                            int rowSpan;
                            int colSpan;
                            cell.IsMerge = ExcelHelper.IsMergeCell(sheet, rowIndex, colIndex, out rowSpan, out colSpan);
                            cell.RowSpan = rowSpan;
                            cell.ColSpan = colSpan;

                            excelEntity[sheetName].CellEntities.Add(cell);
                        }
                    }

                    //获取excel的图片信息
                    excelEntity[sheetName].Pictures = sheet.GetAllPictureInfos();
                }
            }

            return excelEntity;
        }

        /// <summary>
        /// 根据excel实体导出excel ，文件名不需要加后缀
        /// </summary>
        /// <param name="excelEntity"></param>
        /// <param name="fileName"></param>
        public static void ExcelEntityToExcel(Dictionary<string, ExcelEntity> excelEntity, string fileName)
        {
            if (excelEntity == null)
            {
                return;
            }

            IWorkbook workbook = null;

            CellEntity firstCellEntity = excelEntity.First().Value.CellEntities.FirstOrDefault();
            string fileExtend = "";//文件后缀
            if (firstCellEntity != null)
            {
                if (firstCellEntity.Cell is HSSFCell)
                {
                    workbook = new HSSFWorkbook();
                    fileExtend = ".xls";
                }
                else if (firstCellEntity.Cell is XSSFCell)
                {
                    workbook = new XSSFWorkbook();
                    fileExtend = ".xlsx";
                }
            }

            foreach (var sheetName in excelEntity.Keys)
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                List<CellEntity> tempList = excelEntity[sheetName].CellEntities.OrderBy(x => x.FirstRow).ThenBy(x => x.FirstCol).ToList();
                CellEntity firstCell = tempList.First();
                if (firstCell == null)
                {
                    continue;
                }

                int startRow = firstCell.FirstRow;
                int startCol = firstCell.FirstCol;
                int rowCount = excelEntity[sheetName].CellEntities.Select(x => x.FirstRow).Distinct().Count();
                int colCount = excelEntity[sheetName].CellEntities.Select(x => x.FirstCol).Distinct().Count();

                for (int rowIndex = startRow; rowIndex < startRow + rowCount; rowIndex++)
                {//创建单元格
                    IRow row = sheet.CreateRow(rowIndex);
                    for (int colIndex = startCol; colIndex < startCol + colCount; colIndex++)
                    {
                        CellEntity entity = excelEntity[sheetName].CellEntities.Find(x => x.FirstRow == rowIndex && x.FirstCol == colIndex);
                        if (entity != null)
                        {
                            ICell cell = row.CreateCell(colIndex);

                            if (entity.Cell != null)
                            {
                                if (entity.IsMerge)
                                {//合并单元格
                                    sheet.AddMergedRegion(new CellRangeAddress(entity.FirstRow, entity.FirstRow + entity.RowSpan - 1, entity.FirstCol, entity.FirstCol + entity.ColSpan - 1));
                                }

                                cell.SetCellType(entity.Cell.CellType);

                                switch (entity.Cell.CellType)
                                {
                                    case CellType.Numeric:
                                        cell.SetCellValue(entity.Cell.NumericCellValue);
                                        break;
                                    case CellType.Boolean:
                                        cell.SetCellValue(entity.Cell.BooleanCellValue);
                                        break;
                                    case CellType.Formula:
                                    case CellType.Blank:
                                    case CellType.String:
                                    case CellType.Unknown:
                                    case CellType.Error:
                                    default:
                                        cell.SetCellValue(entity.Cell.ToString());
                                        break;
                                } 
                            }

                            //cell.CellStyle = workbook.CreateCellStyle();
                            //cell.CellStyle.CloneStyleFrom(entity.Cell.CellStyle);
                        }
                    }
                }

                if (excelEntity[sheetName].Pictures != null && excelEntity[sheetName].Pictures.Count > 0)
                {
                    //插入图片
                    foreach (var item in excelEntity[sheetName].Pictures)
                    {
                        int pictureIdx = workbook.AddPicture(item.PictureData, PictureType.JPEG);
                        IDrawing patriarch = sheet.CreateDrawingPatriarch();
                        IClientAnchor anchor = patriarch.CreateAnchor(0, 0, 0, 0, item.MinCol, item.MinRow, item.MaxCol, item.MaxRow);
                        IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                byte[] buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(fileName + fileExtend, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }


        }

        private static bool IsMergeCell(ISheet sheet, int rowNum, int colNum, out int rowSpan, out int colSpan)
        {
            rowSpan = 0;
            colSpan = 0;
            if ((rowNum < 0) || (colNum < 0)) return false;
            int rowIndex = rowNum;
            int colIndex = colNum;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            return (rowSpan != 1 || colSpan != 1);
        }

        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
    }

    /// <summary>
    /// excel报表实体
    /// </summary>
    public class ExcelEntity
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        public List<CellEntity> CellEntities { get; set; }
        /// <summary>
        /// 图片信息
        /// </summary>
        public List<PicturesInfo> Pictures { get; set; }
    }

    /// <summary>
    /// 单元格实体
    /// </summary>
    public class CellEntity
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        public ICell Cell { get; set; }
        /// <summary>
        /// 是否合并单元格
        /// </summary>
        public bool IsMerge { get; set; }
        /// <summary>
        /// 起始行
        /// </summary>
        public int FirstRow { get; set; }
        /// <summary>
        /// 起始列
        /// </summary>
        public int FirstCol { get; set; }
        /// <summary>
        /// 跨行数
        /// </summary>
        public int RowSpan { get; set; }
        /// <summary>
        /// 跨列数
        /// </summary>
        public int ColSpan { get; set; }

        public override string ToString()
        {
            return string.Format("起始行：{0}\t起始列：{1}\t是否合并：{2}\t跨行：{3}\t跨列：{4}\t值：{5}",FirstRow,FirstCol,IsMerge,RowSpan,ColSpan,Cell);
        }
    }

    /// <summary>
    /// 图片实体
    /// </summary>
    public class PicturesInfo
    {
        /// <summary>
        /// 起始行
        /// </summary>
        public int MinRow { get; set; }
        /// <summary>
        /// 结束行
        /// </summary>
        public int MaxRow { get; set; }
        /// <summary>
        /// 起始列
        /// </summary>
        public int MinCol { get; set; }
        /// <summary>
        /// 结束列
        /// </summary>
        public int MaxCol { get; set; }
        /// <summary>
        /// 图片数据
        /// </summary>
        public Byte[] PictureData { get; private set; }

        public PicturesInfo(int minRow, int maxRow, int minCol, int maxCol, Byte[] pictureData)
        {
            this.MinRow = minRow;
            this.MaxRow = maxRow;
            this.MinCol = minCol;
            this.MaxCol = maxCol;
            this.PictureData = pictureData;
        }
    }
}