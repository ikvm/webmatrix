namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;

    public class EditorTable
    {
        private HtmlEditor _editor;
        private Interop.IHTMLElement _element;
        private EditorSelection _selection;
        private Interop.IHTMLTable _table;
        private TableLayoutInfo _tableInfo;

        public EditorTable(HtmlEditor editor, EditorSelection selection)
        {
            this._editor = editor;
            this._selection = selection;
            if (!IsTableSelection(selection))
            {
                throw new ArgumentException();
            }
            ICollection items = selection.Items;
            if (items.Count == 1)
            {
                IEnumerator enumerator = items.GetEnumerator();
                enumerator.MoveNext();
                for (Interop.IHTMLElement element = (Interop.IHTMLElement) enumerator.Current; element != null; element = element.GetParentElement())
                {
                    if (string.Compare(element.GetTagName(), "td", true) == 0)
                    {
                        this._element = element;
                        this._table = element.GetParentElement().GetParentElement().GetParentElement() as Interop.IHTMLTable;
                        return;
                    }
                    if (string.Compare(element.GetTagName(), "tr", true) == 0)
                    {
                        this._element = element;
                        this._table = element.GetParentElement().GetParentElement() as Interop.IHTMLTable;
                        return;
                    }
                    if (string.Compare(element.GetTagName(), "tbody", true) == 0)
                    {
                        this._element = element;
                        this._table = element.GetParentElement() as Interop.IHTMLTable;
                        return;
                    }
                    if (string.Compare(element.GetTagName(), "table", true) == 0)
                    {
                        this._element = element;
                        this._table = element as Interop.IHTMLTable;
                        return;
                    }
                }
            }
        }

        public void DeleteTableColumn()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("DeleteTableColumn");
            try
            {
                Interop.IHTMLTableCell cell = null;
                if (string.Compare(this._element.GetTagName(), "td", true) == 0)
                {
                    cell = (Interop.IHTMLTableCell) this._element;
                }
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    num2 += cell.GetColSpan() - 1;
                    int num3 = 0;
                    for (Interop.IHTMLTableCell cell2 = this.TableInfo[num3, num2]; cell2 != null; cell2 = this.TableInfo[num3, num2])
                    {
                        int num4;
                        int num5;
                        Interop.IHTMLTableRow parentElement = ((Interop.IHTMLElement) cell2).GetParentElement() as Interop.IHTMLTableRow;
                        this.TableInfo.GetCellPoint(cell2, out num4, out num5);
                        if (num2 == num5)
                        {
                            int index = num2;
                            parentElement.DeleteCell(index);
                        }
                        else
                        {
                            cell2.SetColSpan(cell2.GetColSpan() - 1);
                        }
                        num3 += cell2.GetRowSpan();
                    }
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void DeleteTableRow()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("DeleteTableRow");
            try
            {
                Interop.IHTMLTableCell cell = null;
                if (string.Compare(this._element.GetTagName(), "td", true) == 0)
                {
                    cell = (Interop.IHTMLTableCell) this._element;
                }
                else if (string.Compare(this._element.GetTagName(), "tr", true) == 0)
                {
                    Interop.IHTMLTableRow row = this._element as Interop.IHTMLTableRow;
                    cell = (Interop.IHTMLTableCell) row.GetCells().Item(0, 0);
                }
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    num += cell.GetRowSpan() - 1;
                    Interop.IHTMLTable table = this.TableInfo.Table;
                    Interop.IHTMLTableRow row2 = table.GetRows().Item(num, num) as Interop.IHTMLTableRow;
                    int num3 = 0;
                    for (Interop.IHTMLTableCell cell2 = this.TableInfo[num, num3]; cell2 != null; cell2 = this.TableInfo[num, num3])
                    {
                        Interop.IHTMLTableRow parentElement = ((Interop.IHTMLElement) cell2).GetParentElement() as Interop.IHTMLTableRow;
                        if (parentElement != row2)
                        {
                            cell2.SetRowSpan(cell2.GetRowSpan() - 1);
                        }
                        num3 += cell2.GetColSpan();
                    }
                    table.DeleteRow(num);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void InsertTableColumn()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("InsertTableColumn");
            try
            {
                Interop.IHTMLTableCell cell = null;
                bool flag = false;
                if (string.Compare(this._element.GetTagName(), "td", true) == 0)
                {
                    cell = (Interop.IHTMLTableCell) this._element;
                }
                else if (string.Compare(this._element.GetTagName(), "tr", true) == 0)
                {
                    Interop.IHTMLTableRow row = this._element as Interop.IHTMLTableRow;
                    int name = row.GetCells().GetLength() - 1;
                    cell = (Interop.IHTMLTableCell) row.GetCells().Item(name, name);
                    flag = true;
                }
                else if (string.Compare(this._element.GetTagName(), "tbody", true) == 0)
                {
                    Interop.IHTMLElementCollection rows = (this._element.GetParentElement() as Interop.IHTMLTable).GetRows();
                    int num2 = rows.GetLength() - 1;
                    Interop.IHTMLTableRow row2 = (Interop.IHTMLTableRow) rows.Item(0, 0);
                    num2 = row2.GetCells().GetLength() - 1;
                    cell = (Interop.IHTMLTableCell) row2.GetCells().Item(num2, num2);
                    flag = true;
                }
                else if (string.Compare(this._element.GetTagName(), "table", true) == 0)
                {
                    Interop.IHTMLElementCollection elements4 = (this._element as Interop.IHTMLTable).GetRows();
                    int num3 = elements4.GetLength() - 1;
                    Interop.IHTMLTableRow row3 = (Interop.IHTMLTableRow) elements4.Item(num3, num3);
                    num3 = row3.GetCells().GetLength() - 1;
                    cell = (Interop.IHTMLTableCell) row3.GetCells().Item(num3, num3);
                    flag = true;
                }
                if (cell != null)
                {
                    int num4;
                    int num5;
                    this.TableInfo.GetCellPoint(cell, out num4, out num5);
                    int num6 = 0;
                    for (Interop.IHTMLTableCell cell2 = this.TableInfo[num6, num5]; cell2 != null; cell2 = this.TableInfo[num6, num5])
                    {
                        int num7;
                        int num8;
                        Interop.IHTMLTableRow parentElement = ((Interop.IHTMLElement) cell2).GetParentElement() as Interop.IHTMLTableRow;
                        this.TableInfo.GetCellPoint(cell2, out num7, out num8);
                        if (flag || (num5 == num8))
                        {
                            int index = num5;
                            if (flag)
                            {
                                index = -1;
                            }
                            parentElement.InsertCell(index).SetRowSpan(cell2.GetRowSpan());
                        }
                        else
                        {
                            cell2.SetColSpan(cell2.GetColSpan() + 1);
                        }
                        num6 += cell2.GetRowSpan();
                    }
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void InsertTableRow()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("InsertTableRow");
            try
            {
                Interop.IHTMLTableCell cell = null;
                bool flag = false;
                if (string.Compare(this._element.GetTagName(), "td", true) == 0)
                {
                    cell = (Interop.IHTMLTableCell) this._element;
                }
                else if (string.Compare(this._element.GetTagName(), "tr", true) == 0)
                {
                    Interop.IHTMLTableRow row = this._element as Interop.IHTMLTableRow;
                    cell = (Interop.IHTMLTableCell) row.GetCells().Item(0, 0);
                }
                else if (string.Compare(this._element.GetTagName(), "tbody", true) == 0)
                {
                    Interop.IHTMLElementCollection rows = (this._element.GetParentElement() as Interop.IHTMLTable).GetRows();
                    int name = rows.GetLength() - 1;
                    Interop.IHTMLTableRow row2 = (Interop.IHTMLTableRow) rows.Item(name, name);
                    cell = (Interop.IHTMLTableCell) row2.GetCells().Item(0, 0);
                    flag = true;
                }
                else if (string.Compare(this._element.GetTagName(), "table", true) == 0)
                {
                    Interop.IHTMLElementCollection elements2 = (this._element as Interop.IHTMLTable).GetRows();
                    int num2 = elements2.GetLength() - 1;
                    Interop.IHTMLTableRow row3 = (Interop.IHTMLTableRow) elements2.Item(num2, num2);
                    cell = (Interop.IHTMLTableCell) row3.GetCells().Item(0, 0);
                    flag = true;
                }
                if (cell != null)
                {
                    int num3;
                    int num4;
                    this.TableInfo.GetCellPoint(cell, out num3, out num4);
                    Interop.IHTMLTableRow parentElement = ((Interop.IHTMLElement) cell).GetParentElement() as Interop.IHTMLTableRow;
                    int rowIndex = parentElement.GetRowIndex();
                    Interop.IHTMLTable table = this.TableInfo.Table;
                    if (flag)
                    {
                        rowIndex = -1;
                    }
                    Interop.IHTMLTableRow row5 = table.InsertRow(rowIndex);
                    int num6 = 0;
                    for (Interop.IHTMLTableCell cell2 = this.TableInfo[num3, num6]; cell2 != null; cell2 = this.TableInfo[num3, num6])
                    {
                        Interop.IHTMLTableRow row6 = ((Interop.IHTMLElement) cell2).GetParentElement() as Interop.IHTMLTableRow;
                        if (flag || (row6 == parentElement))
                        {
                            row5.InsertCell(-1).SetColSpan(cell2.GetColSpan());
                        }
                        else
                        {
                            cell2.SetRowSpan(cell2.GetRowSpan() + 1);
                        }
                        num6 += cell2.GetColSpan();
                    }
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public static bool IsTableSelection(EditorSelection selection)
        {
            selection.SynchronizeSelection();
            if ((selection.SelectionType != EditorSelectionType.TextSelection) || (selection.Length <= 0))
            {
                ICollection items = selection.Items;
                if (items.Count == 1)
                {
                    IEnumerator enumerator = items.GetEnumerator();
                    enumerator.MoveNext();
                    for (Interop.IHTMLElement element = (Interop.IHTMLElement) enumerator.Current; element != null; element = element.GetParentElement())
                    {
                        if (string.Compare(element.GetTagName(), "td", true) == 0)
                        {
                            return true;
                        }
                        if (string.Compare(element.GetTagName(), "tr", true) == 0)
                        {
                            return true;
                        }
                        if (string.Compare(element.GetTagName(), "tbody", true) == 0)
                        {
                            return true;
                        }
                        if (string.Compare(element.GetTagName(), "table", true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void MergeDown()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("MergeDown");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    num += cell.GetRowSpan() - 1;
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num + 1, num2];
                    Interop.IHTMLElement element = (Interop.IHTMLElement) cell2;
                    string innerHTML = element.GetInnerHTML();
                    int rowSpan = cell2.GetRowSpan();
                    Interop.IHTMLTableRow parentElement = element.GetParentElement() as Interop.IHTMLTableRow;
                    int cellIndex = cell2.GetCellIndex();
                    parentElement.DeleteCell(cellIndex);
                    cell.SetRowSpan(cell.GetRowSpan() + rowSpan);
                    this._element.InsertAdjacentHTML("beforeEnd", innerHTML);
                    this._selection.SelectElement(this._element);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void MergeLeft()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("MergeLeft");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num, num2 - 1];
                    Interop.IHTMLElement o = (Interop.IHTMLElement) cell2;
                    string innerHTML = this._element.GetInnerHTML();
                    int colSpan = cell.GetColSpan();
                    Interop.IHTMLTableRow parentElement = this._element.GetParentElement() as Interop.IHTMLTableRow;
                    int cellIndex = cell.GetCellIndex();
                    parentElement.DeleteCell(cellIndex);
                    cell2.SetColSpan(cell2.GetColSpan() + colSpan);
                    o.InsertAdjacentHTML("beforeEnd", innerHTML);
                    this._selection.SelectElement(o);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void MergeRight()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("MergeRight");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    num2 += cell.GetColSpan() - 1;
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num, num2 + 1];
                    Interop.IHTMLElement element = (Interop.IHTMLElement) cell2;
                    string innerHTML = element.GetInnerHTML();
                    int colSpan = cell2.GetColSpan();
                    Interop.IHTMLTableRow parentElement = element.GetParentElement() as Interop.IHTMLTableRow;
                    int cellIndex = cell2.GetCellIndex();
                    parentElement.DeleteCell(cellIndex);
                    cell.SetColSpan(cell.GetColSpan() + colSpan);
                    this._element.InsertAdjacentHTML("beforeEnd", innerHTML);
                    this._selection.SelectElement(this._element);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void MergeUp()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("MergeUp");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num - 1, num2];
                    Interop.IHTMLElement o = (Interop.IHTMLElement) cell2;
                    string innerHTML = this._element.GetInnerHTML();
                    int rowSpan = cell.GetRowSpan();
                    Interop.IHTMLTableRow parentElement = this._element.GetParentElement() as Interop.IHTMLTableRow;
                    int cellIndex = cell.GetCellIndex();
                    parentElement.DeleteCell(cellIndex);
                    cell2.SetRowSpan(cell2.GetRowSpan() + rowSpan);
                    o.InsertAdjacentHTML("beforeEnd", innerHTML);
                    this._selection.SelectElement(o);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void SplitHorizontal()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("SplitHorizontal");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    int colSpan = cell.GetColSpan();
                    if (colSpan > 1)
                    {
                        cell.SetColSpan(colSpan - 1);
                    }
                    else
                    {
                        int num4 = 0;
                        for (Interop.IHTMLTableCell cell2 = this.TableInfo[num4, num2]; cell2 != null; cell2 = this.TableInfo[num4, num2])
                        {
                            if (cell2 != cell)
                            {
                                cell2.SetColSpan(cell2.GetColSpan() + 1);
                            }
                            num4 += cell2.GetRowSpan();
                        }
                    }
                    Interop.IHTMLTableCell o = (((Interop.IHTMLElement) cell).GetParentElement() as Interop.IHTMLTableRow).InsertCell(cell.GetCellIndex() + 1);
                    o.SetRowSpan(cell.GetRowSpan());
                    Interop.IHTMLTxtRange mSHTMLSelection = this._selection.MSHTMLSelection as Interop.IHTMLTxtRange;
                    if (mSHTMLSelection != null)
                    {
                        for (int i = 1; i > 0; i = mSHTMLSelection.MoveEnd("character", 1))
                        {
                        }
                        string innerHTML = this._element.GetInnerHTML();
                        string text = mSHTMLSelection.GetText();
                        if ((text != null) && (text.Length > 0))
                        {
                            int length = innerHTML.LastIndexOf(text);
                            if (length != -1)
                            {
                                this._element.SetInnerHTML(innerHTML.Substring(0, length));
                                ((Interop.IHTMLElement) o).SetInnerHTML(text);
                            }
                        }
                    }
                    this._selection.SelectElement(o);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public void SplitVertical()
        {
            BatchedUndoUnit unit = this._editor.OpenBatchUndo("SplitVertical");
            try
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    int rowSpan = cell.GetRowSpan();
                    if (rowSpan > 1)
                    {
                        cell.SetRowSpan(rowSpan - 1);
                    }
                    else
                    {
                        int num4 = 0;
                        for (Interop.IHTMLTableCell cell2 = this.TableInfo[num, num4]; cell2 != null; cell2 = this.TableInfo[num, num4])
                        {
                            if (cell2 != cell)
                            {
                                cell2.SetRowSpan(cell2.GetRowSpan() + 1);
                            }
                            num4 += cell2.GetColSpan();
                        }
                    }
                    Interop.IHTMLTableRow parentElement = ((Interop.IHTMLElement) cell).GetParentElement() as Interop.IHTMLTableRow;
                    Interop.IHTMLTable table = this.TableInfo.Table;
                    int name = parentElement.GetRowIndex() + cell.GetRowSpan();
                    Interop.IHTMLTableRow row2 = table.GetRows().Item(name, name) as Interop.IHTMLTableRow;
                    Interop.IHTMLTableCell o = null;
                    bool flag = false;
                    if ((row2 != null) && (rowSpan > 1))
                    {
                        int num6 = num2;
                        while (num6 >= 0)
                        {
                            Interop.IHTMLTableCell cell4 = this.TableInfo[name, num6];
                            Interop.IHTMLTableRow row3 = ((Interop.IHTMLElement) cell4).GetParentElement() as Interop.IHTMLTableRow;
                            if (row3 == row2)
                            {
                                break;
                            }
                            num6--;
                        }
                        if (parentElement.GetCells().GetLength() > row2.GetCells().GetLength())
                        {
                            int index = 0;
                            if (num6 >= 0)
                            {
                                Interop.IHTMLTableCell cell5 = this.TableInfo[name, num6];
                                index = cell5.GetCellIndex() + 1;
                            }
                            o = row2.InsertCell(index);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        o = table.InsertRow(name).InsertCell(-1);
                    }
                    o.SetColSpan(cell.GetColSpan());
                    Interop.IHTMLTxtRange mSHTMLSelection = this._selection.MSHTMLSelection as Interop.IHTMLTxtRange;
                    if (mSHTMLSelection != null)
                    {
                        for (int i = 1; i > 0; i = mSHTMLSelection.MoveEnd("character", 1))
                        {
                        }
                        string innerHTML = this._element.GetInnerHTML();
                        string text = mSHTMLSelection.GetText();
                        if ((text != null) && (text.Length > 0))
                        {
                            int length = innerHTML.LastIndexOf(text);
                            if (length != -1)
                            {
                                this._element.SetInnerHTML(innerHTML.Substring(0, length));
                                ((Interop.IHTMLElement) o).SetInnerHTML(text);
                            }
                        }
                    }
                    this._selection.SelectElement(o);
                    this._tableInfo = null;
                }
            }
            finally
            {
                unit.Close();
            }
        }

        public bool CanDeleteTableColumn
        {
            get
            {
                return (string.Compare(this._element.GetTagName(), "td", true) == 0);
            }
        }

        public bool CanDeleteTableRow
        {
            get
            {
                if (string.Compare(this._element.GetTagName(), "td", true) != 0)
                {
                    return (string.Compare(this._element.GetTagName(), "tr", true) == 0);
                }
                return true;
            }
        }

        public bool CanInsertTableColumn
        {
            get
            {
                return true;
            }
        }

        public bool CanInsertTableRow
        {
            get
            {
                return true;
            }
        }

        public bool CanMergeDown
        {
            get
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num + cell.GetRowSpan(), num2];
                    if (cell2 != null)
                    {
                        int num3;
                        int num4;
                        this.TableInfo.GetCellPoint(cell2, out num3, out num4);
                        return ((num4 == num2) && (cell2.GetColSpan() == cell.GetColSpan()));
                    }
                }
                return false;
            }
        }

        public bool CanMergeLeft
        {
            get
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num, num2 - 1];
                    if (cell2 != null)
                    {
                        int num3;
                        int num4;
                        this.TableInfo.GetCellPoint(cell2, out num3, out num4);
                        return ((num3 == num) && (cell2.GetRowSpan() == cell.GetRowSpan()));
                    }
                }
                return false;
            }
        }

        public bool CanMergeRight
        {
            get
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num, num2 + cell.GetColSpan()];
                    if (cell2 != null)
                    {
                        int num3;
                        int num4;
                        this.TableInfo.GetCellPoint(cell2, out num3, out num4);
                        return ((num3 == num) && (cell2.GetRowSpan() == cell.GetRowSpan()));
                    }
                }
                return false;
            }
        }

        public bool CanMergeUp
        {
            get
            {
                Interop.IHTMLTableCell cell = this._element as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int num;
                    int num2;
                    this.TableInfo.GetCellPoint(cell, out num, out num2);
                    Interop.IHTMLTableCell cell2 = this.TableInfo[num - 1, num2];
                    if (cell2 != null)
                    {
                        int num3;
                        int num4;
                        this.TableInfo.GetCellPoint(cell2, out num3, out num4);
                        return ((num4 == num2) && (cell2.GetColSpan() == cell.GetColSpan()));
                    }
                }
                return false;
            }
        }

        public bool CanSplitHorizontal
        {
            get
            {
                return (string.Compare(this._element.GetTagName(), "td", true) == 0);
            }
        }

        public bool CanSplitVertical
        {
            get
            {
                return (string.Compare(this._element.GetTagName(), "td", true) == 0);
            }
        }

        public Interop.IHTMLElement SelectedElement
        {
            get
            {
                return this._element;
            }
        }

        private TableLayoutInfo TableInfo
        {
            get
            {
                if (this._tableInfo == null)
                {
                    this._tableInfo = new TableLayoutInfo(this._table);
                    Interop.IHTMLElementCollection rows = this._table.GetRows();
                    int length = rows.GetLength();
                    for (int i = 0; i < length; i++)
                    {
                        Interop.IHTMLElementCollection cells = (rows.Item(i, i) as Interop.IHTMLTableRow).GetCells();
                        int num3 = cells.GetLength();
                        for (int j = 0; j < num3; j++)
                        {
                            Interop.IHTMLTableCell cell = cells.Item(j, j) as Interop.IHTMLTableCell;
                            this._tableInfo.AddCell(i, cell);
                        }
                    }
                }
                return this._tableInfo;
            }
        }

        private class TableLayoutInfo
        {
            private IDictionary _cells;
            private ArrayList _rows;
            private Interop.IHTMLTable _table;

            public TableLayoutInfo(Interop.IHTMLTable table)
            {
                this._table = table;
                int length = table.GetRows().GetLength();
                this._rows = new ArrayList(length);
                for (int i = 0; i < length; i++)
                {
                    this._rows.Add(new ArrayList());
                }
                this._cells = new HybridDictionary();
            }

            public void AddCell(int rowIndex, Interop.IHTMLTableCell cell)
            {
                ArrayList list = (ArrayList) this._rows[rowIndex];
                int num = 0;
                while ((num < list.Count) && (list[num] != null))
                {
                    num++;
                }
                for (int i = 0; i < cell.GetColSpan(); i++)
                {
                    int index = num + i;
                    this.EnsureIndex(list, index);
                    list[index] = cell;
                    if (!this._cells.Contains(cell))
                    {
                        this._cells[cell] = new Pair(rowIndex, index);
                    }
                    for (int j = 1; j < cell.GetRowSpan(); j++)
                    {
                        if ((rowIndex + 1) >= this._rows.Count)
                        {
                            this._rows.Add(new ArrayList());
                        }
                        ArrayList list2 = (ArrayList) this._rows[rowIndex + j];
                        this.EnsureIndex(list2, index);
                        list2[index] = cell;
                        if (!this._cells.Contains(cell))
                        {
                            this._cells[cell] = new Pair(rowIndex + j, index);
                        }
                    }
                }
            }

            private void EnsureIndex(ArrayList list, int index)
            {
                int num = index - (list.Count - 1);
                for (int i = 0; i < num; i++)
                {
                    list.Add(null);
                }
            }

            public void GetCellPoint(Interop.IHTMLTableCell cell, out int row, out int col)
            {
                Pair pair = (Pair) this._cells[cell];
                if (pair != null)
                {
                    row = (int) pair.First;
                    col = (int) pair.Second;
                }
                else
                {
                    row = -1;
                    col = -1;
                }
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this._rows.Count; i++)
                {
                    ArrayList list = (ArrayList) this._rows[i];
                    for (int j = 0; j < list.Count; j++)
                    {
                        builder.Append("----------");
                    }
                    builder.Append(Environment.NewLine);
                    for (int k = 0; k < list.Count; k++)
                    {
                        Interop.IHTMLElement element = (Interop.IHTMLElement) list[k];
                        if (element != null)
                        {
                            builder.Append("|");
                            string innerHTML = element.GetInnerHTML();
                            if (innerHTML != null)
                            {
                                builder.Append(innerHTML.Substring(0, Math.Min(innerHTML.Length, 8)));
                            }
                            else
                            {
                                builder.Append("empty");
                            }
                        }
                        else
                        {
                            builder.Append("|   null  ");
                        }
                    }
                    builder.Append("|");
                    builder.Append(Environment.NewLine);
                }
                return builder.ToString();
            }

            public Interop.IHTMLTableCell this[int row, int col]
            {
                get
                {
                    if ((row < this._rows.Count) && (row >= 0))
                    {
                        ArrayList list = (ArrayList) this._rows[row];
                        if ((col < list.Count) && (col >= 0))
                        {
                            return (Interop.IHTMLTableCell) list[col];
                        }
                    }
                    return null;
                }
            }

            public Interop.IHTMLTable Table
            {
                get
                {
                    return this._table;
                }
            }
        }
    }
}

