﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace Mshan.Utilities
{
    public class WordHelper
    {
        private _Application wordApp = null;
        private _Document wordDoc = null;
        public _Application Application
        {
            get
            {
                return wordApp;
            }
            set
            {
                wordApp = value;
            }
        }
        public _Document Document
        {
            get
            {
                return wordDoc;
            }
            set
            {
                wordDoc = value;
            }
        }

        // 通过模板创建新文档
        public void CreateNewDocument()
        {

                killWinWordProcess();
                wordApp = new ApplicationClass();
                wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                wordApp.Visible = false;
                object missing = System.Reflection.Missing.Value;
                wordDoc = wordApp.Documents.Add(
               ref missing, ref missing, ref missing, ref missing);
                wordDoc.SpellingChecked = false;//关闭拼写检查
                wordDoc.ShowSpellingErrors = false;//关闭显示拼写错误提示框 
        }

        public void OpenDocument(string fullFileName)
        {
            wordApp = new ApplicationClass();
            wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordApp.Visible = false;
            wordDoc = wordApp.Documents.Open(fullFileName);
            wordDoc.SpellingChecked = false;//关闭拼写检查
            wordDoc.ShowSpellingErrors = false;//关闭显示拼写错误提示框 
        }

        // 保存新文件
        public void SaveDocument(string filePath)
        {
            wordApp.Visible = false; //不可见直接保存

            wordDoc.SaveAs(filePath);//保存

            wordDoc.Close(true);//关闭

            wordApp.Quit();//释放Word进程
        }
        // 保存新文件
        public void SaveAndQuitDocument(string filePath)
        {
            wordApp.Visible = false; //不可见直接保存

            wordDoc.SaveAs(filePath);//保存

            wordDoc.Close(true);//关闭

            wordApp.Quit();//释放Word进程
        }
        // 在书签处插入值
        public bool InsertValue(string bookmark, string value)
        {
            object bkObj = bookmark;
            if (wordApp.ActiveDocument.Bookmarks.Exists(bookmark))
            {
                wordApp.ActiveDocument.Bookmarks.get_Item(ref bkObj).Select();
                wordApp.Selection.TypeText(value);
                return true;
            }
            return false;
        }

        // 插入表格,bookmark书签
        public Table InsertTable(int rows, int columns, float width)
        {
            Table table = wordDoc.Paragraphs.Last.Range.Tables.Add(wordDoc.Paragraphs.Last.Range, rows, columns);
            table.PreferredWidth = width;
            //设置边线可见 
            table.Borders.Enable = 1;
            return table;
        }


        // 合并单元格 表id,开始行号,开始列号,结束行号,结束列号
        public void MergeCell(int n, int row1, int column1, int row2, int column2)
        {
            wordDoc.Content.Tables[n].Cell(row1, column1).Merge(wordDoc.Content.Tables[n].Cell(row2, column2));
        }

        // 合并单元格 表名,开始行号,开始列号,结束行号,结束列号
        public void MergeCell(Microsoft.Office.Interop.Word.Table table, int row1, int column1, int row2, int column2)
        {
            table.Cell(row1, column1).Merge(table.Cell(row2, column2));
        }

        // 设置表格内容对齐方式 Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)Microsoft.Office.Interop.Word.Table table
        public void SetParagraph_Table(int n, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }

        // 设置单元格内容对齐方式
        public void SetParagraph_Table(int n, int row, int column, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }

        }


        // 设置表格字体
        public void SetFont_Table(Microsoft.Office.Interop.Word.Table table, string fontName, double size)
        {
            if (size != 0)
            {
                table.Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                table.Range.Font.Name = fontName;
            }
        }

        // 设置单元格字体
        public void SetFont_Table(int n, int row, int column, string fontName, double size, int bold)
        {
            if (size != 0)
            {
                wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Name = fontName;
            }
            wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Bold = bold;// 0 表示不是粗体，其他值都是
        }

        // 是否使用边框,n表格的序号,use是或否
        // 该处边框参数可以用int代替bool可以让方法更全面
        // 具体值方法中介绍
        public void UseBorder(int n, bool use)
        {
            if (use)
            {
                wordDoc.Content.Tables[n].Borders.Enable = 1;
                //允许有边框，默认没有边框(为0时无边框，1为实线边框，2、3为虚线边框，以后的数字没试过)
            }
            else
            {
                wordDoc.Content.Tables[n].Borders.Enable = 0;
            }
        }

        // 给表格插入一行,n表格的序号从1开始记
        public void AddRow(int n)
        {
            object miss = System.Reflection.Missing.Value;
            wordDoc.Content.Tables[n].Rows.Add(ref miss);
        }

        // 给表格添加一行
        public void AddRow(Microsoft.Office.Interop.Word.Table table)
        {
            object miss = System.Reflection.Missing.Value;
            table.Rows.Add(ref miss);
        }

        // 给表格插入rows行,n为表格的序号
        public void AddRow(int n, int rows)
        {
            object miss = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < rows; i++)
            {
                table.Rows.Add(ref miss);
            }
        }

        // 删除表格第rows行,n为表格的序号
        public void DeleteRow(int n, int row)
        {
            Microsoft.Office.Interop.Word.Table table = wordDoc.Content.Tables[n];
            table.Rows[row].Delete();
        }

        // 给表格中单元格插入元素，table所在表格，row行号，column列号，value插入的元素
        public void InsertCell(Microsoft.Office.Interop.Word.Table table, int row, int column, string value)
        {
            table.Cell(row, column).Range.Text = value;
        }

        // 给表格中单元格插入元素，n表格的序号从1开始记，row行号，column列号，value插入的元素
        public void InsertCell(int n, int row, int column, string value)
        {
            wordDoc.Content.Tables[n].Cell(row, column).Range.Text = value;
        }

        // 给表格插入一行数据，n为表格的序号，row行号，columns列数，values插入的值
        public void InsertCell(int n, int row, int columns, string[] values)
        {
            Microsoft.Office.Interop.Word.Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < columns; i++)
            {
                table.Cell(row, i + 1).Range.Text = values[i];
            }
        }

        // 插入图片
        public void InsertPicture(string bookmark, string picturePath, float width, float hight)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            Object linkToFile = false;       //图片是否为外部链接
            Object saveWithDocument = true;  //图片是否随文档一起保存 
            object range = wordDoc.Bookmarks.get_Item(ref oStart).Range;//图片插入位置
            wordDoc.InlineShapes.AddPicture(picturePath, ref linkToFile, ref saveWithDocument, ref range);
            wordDoc.Application.ActiveDocument.InlineShapes[1].Width = width;   //设置图片宽度
            wordDoc.Application.ActiveDocument.InlineShapes[1].Height = hight;  //设置图片高度
        }

        // 插入一段文字,text为文字内容
        public void InsertText(string text)
        {
            wordDoc.Paragraphs.Last.Range.Text = text+"\n";
            //Object label = WdCaptionLabelID.wdCaptionFigure;
            //Object  title = "标题";
            //wordDoc.Paragraphs.Last.Range.InsertCaption(ref label,ref title);
        }

        // 插入一段文字,text为文字内容
        public void InsertTitle(string text,WdOutlineLevel outlineLevel)
        {
            Object label = WdCaptionLabelID.wdCaptionFigure;
            wordDoc.Paragraphs.Last.Format.OutlineLevel = outlineLevel;
            wordDoc.Paragraphs.Last.Range.Text = text+"\n";
        }
        public void SetFont(float fontSize)
        {
            SetFont(fontSize, WdParagraphAlignment.wdAlignParagraphLeft);
        }
        public void SetFont(float fontSize, WdParagraphAlignment alignment)
        {
            wordDoc.Paragraphs.Last.Range.Font.Size = fontSize;
            wordDoc.Paragraphs.Last.Range.Font.Name = "微软雅黑";
            wordDoc.Paragraphs.Last.Range.ParagraphFormat.Alignment = alignment;
        }
        // 杀掉winword.exe进程
        public void killWinWordProcess()
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("WINWORD");
            foreach (System.Diagnostics.Process process in processes)
            {
                bool b = process.MainWindowTitle == "";
                if (process.MainWindowTitle == "")
                {
                    process.Kill();
                }
            }
        }
    }
}
