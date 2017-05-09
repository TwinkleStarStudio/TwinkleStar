using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TwinkleStar.Common.Excel
{
    /// <summary>
    /// Excel生成操作类
    /// </summary>
    public class NPOIHelper
    {
        /// <summary>
        /// 需要初始化设置的表头的列名（必须设置）
        /// </summary>
        public static SortedList ListColumnsName;

        /// <summary>
        /// 最后一行总计列
        /// </summary>
        public static SortedList TotalColummsName;

        #region 导出

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static void ExportExcel(IList list, string filePath)
        {
            DataTable dtSource = DataHelper.ToDataTable(list);
            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));
            IWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, filePath);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static void ExportExcel(DataTable dtSource, string filePath)
        {
            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, filePath);
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static void ExportExcel(IList list, Stream excelStream)
        {
            DataTable dtSource = DataHelper.ToDataTable(list);

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static void ExportExcel(DataTable dtSource, Stream excelStream)
        {
            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static void ExportExcel(HttpResponse response, IList list, Stream excelStream, string fileName)
        {
            DataTable dtSource = DataHelper.ToDataTable(list);

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();

            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);

            response.Clear();
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}({1}).xls", fileName, DateTime.Now.ToString("yyyyMMdd")));
            response.Charset = "utf-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            response.ContentType = "application/vnd.xls";
            response.BinaryWrite(((MemoryStream)excelStream).ToArray());
            response.End();

        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="response"></param>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public static void ExportExcel(HttpResponse response, IList list, string fileName)
        {
            MemoryStream excelStream = new MemoryStream();

            DataTable dtSource = DataHelper.ToDataTable(list);

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();

            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);

            response.Clear();
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}({1}).xls", fileName, DateTime.Now.ToString("yyyyMMdd")));
            response.Charset = "utf-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            response.ContentType = "application/vnd.xls";
            response.BinaryWrite((excelStream).ToArray());

            excelStream.Close();
            excelStream.Dispose();
            response.End();

        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="response"></param>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public static void ExportExcel(HttpResponseBase response, IList list, string fileName)
        {
            MemoryStream excelStream = new MemoryStream();

            DataTable dtSource = DataHelper.ToDataTable(list);

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();

            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);

            response.Clear();
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}({1}).xls", fileName, DateTime.Now.ToString("yyyyMMdd")));
            response.Charset = "utf-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            response.ContentType = "application/vnd.xls";
            response.BinaryWrite((excelStream).ToArray());

            excelStream.Close();
            excelStream.Dispose();
            response.End();

        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="response"></param>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public static void ExportExcel(HttpResponse response, DataTable dtSource, string fileName)
        {
            MemoryStream excelStream = new MemoryStream();

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            IWorkbook excelWorkbook = CreateExcelFile();

            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, excelStream);

            response.Clear();
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}({1}).xls", fileName, DateTime.Now.ToString("yyyyMMdd")));
            response.Charset = "utf-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            response.ContentType = "application/vnd.xls";
            response.BinaryWrite((excelStream).ToArray());

            excelStream.Close();

            excelStream.Dispose();

            response.End();

        } 
        #endregion

        #region 导入

        /// <summary>    
        /// 由Excel导入到DataTable    
        /// </summary>    
        /// <param name="excelFileStream">Excel文件流</param>    
        /// <param name="sheetName">Excel工作表名称</param>    
        /// <param name="headerRowIndex">Excel表头行索引</param>    
        /// <returns>DataTable</returns>    
        public static DataTable ImportExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
        {
            try
            {
                IWorkbook workbook;
                try
                {
                    workbook = new XSSFWorkbook(excelFileStream);//2007版本
                }
                catch
                {
                    workbook = new HSSFWorkbook(excelFileStream);//2003版本
                }
                
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                DataTable table = new DataTable();
                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        dataRow[j] = row.GetCell(j) == null ? "" : row.GetCell(j).ToString().Trim();
                    }

                    //判断正行数据是否有效
                    bool key = false;
                    for (int ii = 0; ii < cellCount; ii++)
                    {
                        if (!string.IsNullOrEmpty(dataRow[ii].ToString()))
                        {
                            key = true;
                            break;
                        }
                    }
                    if (key)
                    {
                        table.Rows.Add(dataRow);
                    }
                }
                workbook = null;
                sheet = null;
                return table;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                excelFileStream.Close();
            }
        }

        #endregion

        #region private

        /// <summary>
        /// CellBorder设置（表头）
        /// </summary>
        /// <param name="excelWorkbook"></param>
        /// <returns></returns>
        private static ICellStyle BorderCellStyle(IWorkbook excelWorkbook)
        {
            ICellStyle style = excelWorkbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.Alignment = HorizontalAlignment.Center;
            style.FillBackgroundColor = (short)999;
            return style;
        }
        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        private static void SaveExcelFile(IWorkbook excelWorkBook, string filePath)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filePath, FileMode.Create);
                excelWorkBook.Write(file);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        private static void SaveExcelFile(IWorkbook excelWorkBook, Stream excelStream)
        {
            try
            {
                excelWorkBook.Write(excelStream);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="filePath"></param>
        private static IWorkbook CreateExcelFile()
        {
            IWorkbook hssfworkbook = new HSSFWorkbook();
            return hssfworkbook;
        }
        /// <summary>
        /// 创建excel表头
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="excelSheet"></param>
        private static void CreateHeader(ISheet excelSheet, ICellStyle style)
        {
            int cellIndex = 0;
            IRow newRow = excelSheet.CreateRow(0);
            //循环导出列
            foreach (DictionaryEntry de in ListColumnsName)
            {
                ICell newCell = newRow.CreateCell(cellIndex);
                newCell.SetCellValue(de.Value.ToString());
                //按表头文字的宽度
                excelSheet.SetColumnWidth(cellIndex, de.Value.ToString().Length * 650);
                newCell.CellStyle = style;
                cellIndex++;
            }
        }
        /// <summary>
        /// 创建Total列
        /// </summary>
        /// <param name="excelSheet"></param>
        private static void CreateTotalFooter(ISheet excelSheet, int lastRow)
        {
            //无Total列
            if (TotalColummsName == null || TotalColummsName.Count == 0)
                return;

            string firstTotalColumnName = (TotalColummsName.GetKeyList()[0]).ToString();

            int cellIndex = 0;
            foreach (DictionaryEntry de in ListColumnsName)
            {
                if (de.Key.ToString() == firstTotalColumnName)
                {
                    break;
                }
                cellIndex++;
            }

            if (cellIndex == -1)
                return;

            IRow newRow = excelSheet.CreateRow(lastRow);

            //循环导出Total列
            foreach (DictionaryEntry footColumn in TotalColummsName)
            {
                ICell newCell = newRow.CreateCell(cellIndex);
                newCell.SetCellValue(footColumn.Value.ToString());
                cellIndex++;
            }
            
        }

        /// <summary>
        /// 插入数据行
        /// </summary>
        private static void InsertRow(DataTable dtSource, IWorkbook excelWorkbook)
        {
            int rowCount = 0;
            int sheetCount = 1;
            ISheet newsheet = null;

            //循环数据源导出数据集
            newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount);

            ICellStyle style = BorderCellStyle(excelWorkbook);

            //设置表头
            CreateHeader(newsheet, style);
            
            foreach (DataRow dr in dtSource.Rows)
            {
                rowCount++;
                //超出65530条数据 创建新的工作簿
                if (rowCount == 65530)
                {
                    rowCount = 1;
                    sheetCount++;
                    newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount);
                    CreateHeader(newsheet, style);
                }

                IRow newRow = newsheet.CreateRow(rowCount);
                InsertCell(dtSource, dr, newRow, newsheet, excelWorkbook);
            }

            //设置最后一列：Total列(最后一个sheet有总计)
            CreateTotalFooter(newsheet, dtSource.Rows.Count + 1);

        }
        /// <summary>
        /// 导出数据行
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="drSource"></param>
        /// <param name="currentExcelRow"></param>
        /// <param name="excelSheet"></param>
        /// <param name="excelWorkBook"></param>
        private static void InsertCell(DataTable dtSource, DataRow drSource, IRow currentExcelRow, ISheet excelSheet, IWorkbook excelWorkBook)
        {
            for (int cellIndex = 0; cellIndex < ListColumnsName.Count; cellIndex++)
            {
                //列名称
                string columnsName = ListColumnsName.GetKey(cellIndex).ToString();
                ICell newCell = null;
                System.Type rowType = drSource[columnsName].GetType();
                string drValue = drSource[columnsName].ToString().Trim();
                switch (rowType.ToString())
                {
                    case "System.String"://字符串类型
                        drValue = drValue.Replace("&", "&");
                        drValue = drValue.Replace(">", ">");
                        drValue = drValue.Replace("<", "<");
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(drValue);
                        break;
                    case "System.DateTime"://日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(dateV);

                        //格式化显示
                        ICellStyle cellStyle = excelWorkBook.CreateCellStyle();
                        IDataFormat format = excelWorkBook.CreateDataFormat();
                        cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
                        newCell.CellStyle = cellStyle;

                        break;
                    case "System.Boolean"://布尔型
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(boolV);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(intV.ToString());
                        break;
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(doubV);
                        break;
                    case "System.DBNull"://空值处理
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue("");
                        break;
                    default:
                        throw (new Exception(rowType.ToString() + "：类型数据无法处理!"));
                }
            }
        }
        #endregion
    }
    //排序实现接口 不进行排序 根据添加顺序导出
    public class NoSort : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            return -1;
        }
    }
}
