using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Matrix
{
    public enum CmdID
    {
        MdiLayout_Cascade = 400,
        MdiLayout_TileHorizontal=0x191,
        MdiLayout_TileVertical = 0x192,
        MdiLayout_CloseAllChildWindows = 0x192,
        ToggleToolBox = 0xd6,
    }






                this.AddMenuItem(this.menuItemTable1, commandGroup, 1, "&New File...", "Create a new file", Shortcut.CtrlN, 1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 2, "&Open Files...", "Open existing files", Shortcut.CtrlO, 2);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 3, "&Save File", "Save the current file", Shortcut.CtrlS, 3);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 4, "Save File &As...", "Save the current file to a new location", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 5, "&Close", "Close the current file", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 10, "New Pro&ject...", "Create a new project", Shortcut.None, 0x37);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 6, "&Print...", "Print the current file", Shortcut.CtrlP, 0x1f);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 7, "P&rint Preview...", "Print-preview the current file", Shortcut.None, 30);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 8, "Pr&int Settings...", "Set up printer defaults and settings...", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 9, "E&xit", "Close the application", Shortcut.None, -1);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.menuItemTable1, commandGroup, num + 20, "", "Open this file", Shortcut.None, -1);
                }
                for (num = 0; num < 5; num++)
                {
                    this.AddMenuItem(this.menuItemTable1, commandGroup, num + 40, "", "Open this project", Shortcut.None, -1);
                }
                this.AddMenuItem(this.menuItemTable1, commandGroup, 100, "&Undo", "Undo the last change", Shortcut.CtrlZ, 4);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x65, "&Redo", "Reverse the last undo", Shortcut.CtrlY, 5);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x6a, "Select &All", "Select everything in the file", Shortcut.CtrlA, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x6b, "&Go To...", "Go to the specific line in the current file", Shortcut.CtrlG, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x6c, "&Find...", "Search for the specified text", Shortcut.CtrlF, 40);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 180, "Find &Next", "Search for the next occurence", Shortcut.F3, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x6d, "R&eplace...", "Search for and replace the specified text", Shortcut.CtrlH, 0x22);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 120, "A&dd Snippet", "Copy the selected text as a snippet", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 250, "For&mat Document", "Format the entire document", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 260, "Ed&it Tag", "Edit the source of the selected tag", Shortcut.CtrlT, 0x36);
                this.AddMenuItem(this.menuItemTable2, type2, 240, "Edit Temp&lates...", "Edit the templates of the selected control", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x8d, "C&omment Selection", "Comment the selected text", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x8e, "U&ncomment Selection", "Uncomment the selected text", Shortcut.None, -1);
                for (num = 0; num < 4; num++)
                {
                    this.AddMenuItem(this.menuItemTable1, commandGroup, num + 200, "", "Switch to this editor view", Shortcut.None, -1);
                }
                this.AddMenuItem(this.menuItemTable1, commandGroup, 210, "&Start...", "Run the current file", Shortcut.F5, 0x3e);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0xd6, "&Toolbox", "Toggle the Toolbox Window", Shortcut.F2, 0x43);
                this.AddMenuItem(this.menuItemTable2, type2, 1, "&Glyphs", "Toggle glyphs in the current file", Shortcut.None, 0x26);
                this.AddMenuItem(this.menuItemTable2, type2, 2, "&Borders", "Toggle design-time borders in the current file", Shortcut.None, 0x25);
                this.AddMenuItem(this.menuItemTable2, type2, 3, "G&rid", "Toggle the design-time grid in the current file", Shortcut.None, 0x23);
                this.AddMenuItem(this.menuItemTable2, type2, 4, "S&nap to Grid", "Toggle the snap to grid behavior in the current file", Shortcut.None, 0x24);
                this.AddMenuItem(this.menuItemTable2, type2, 100, "&Bold", "Make the selected text bold", Shortcut.CtrlB, 12);
                this.AddMenuItem(this.menuItemTable2, type2, 0x65, "&Italic", "Make the selected text italic", Shortcut.CtrlI, 13);
                this.AddMenuItem(this.menuItemTable2, type2, 0x66, "&Underline", "Underline the selected text", Shortcut.CtrlU, 14);
                this.AddMenuItem(this.menuItemTable2, type2, 0x67, "&Superscript", "Convert the selected text into superscript text", Shortcut.None, 15);
                this.AddMenuItem(this.menuItemTable2, type2, 0x68, "Subscri&pt", "Convert the selected text into subscript text", Shortcut.None, 0x10);
                this.AddMenuItem(this.menuItemTable2, type2, 0x69, "S&trike", "Strikeout the selected text", Shortcut.None, 0x18);
                this.AddMenuItem(this.menuItemTable2, type2, 0x6a, "&Foreground Color...", "Select the foreground color", Shortcut.None, 0x20);
                this.AddMenuItem(this.menuItemTable2, type2, 0x6b, "B&ackground Color...", "Select the backgbround color", Shortcut.None, 0x21);
                this.AddMenuItem(this.menuItemTable2, type2, 0x6c, "Align &Left", "Align the selected text to the left", Shortcut.None, 0x11);
                this.AddMenuItem(this.menuItemTable2, type2, 110, "Align &Center", "Center the selected text", Shortcut.None, 0x12);
                this.AddMenuItem(this.menuItemTable2, type2, 0x6d, "Align &Right", "Align the selected text to the right", Shortcut.None, 0x13);
                this.AddMenuItem(this.menuItemTable2, type2, 0x6f, "&Ordered List", "Convert the selected text into ordered numbered list", Shortcut.None, 20);
                this.AddMenuItem(this.menuItemTable2, type2, 0x70, "U&nordered List", "Convert the selected text into bulleted list", Shortcut.None, 0x15);
                this.AddMenuItem(this.menuItemTable2, type2, 0x71, "In&dent", "Indent the selected text in", Shortcut.None, 0x17);
                this.AddMenuItem(this.menuItemTable2, type2, 0x72, "Unind&ent", "Unindent the selected text out", Shortcut.None, 0x16);
                this.AddMenuItem(this.menuItemTable2, type2, 0x7b, "St&yle...", "Edit the style and formatting attributes of the selection", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 130, "&Normal", "Apply the normal format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x83, "&Formatted", "Apply the formatted format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x84, "Heading &1", "Apply the heading 1 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x85, "Heading &2", "Apply the heading 2 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x86, "Heading &3", "Apply the heading 3 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x87, "Heading &4", "Apply the heading 4 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x88, "Heading &5", "Apply the heading 5 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x89, "Heading &6", "Apply the heading 6 format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0x8a, "&Paragraph", "Apply the paragraph format to the selected text.", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 140, "&Absolute Position", "Toggle absolute positioning of the selected elements", Shortcut.None, 0x19);
                this.AddMenuItem(this.menuItemTable2, type2, 0x8f, "Align &Left Edges", "Align the left edges of the selected elements", Shortcut.None, 0x2b);
                this.AddMenuItem(this.menuItemTable2, type2, 0x90, "Align &Center", "Align the centers of the selected elements", Shortcut.None, 0x2c);
                this.AddMenuItem(this.menuItemTable2, type2, 0x91, "Align &Right Edges", "Align the right edges of the selected elements", Shortcut.None, 0x2d);
                this.AddMenuItem(this.menuItemTable2, type2, 0x92, "Align &Top Edges", "Align the top edges of the selected elements", Shortcut.None, 0x2e);
                this.AddMenuItem(this.menuItemTable2, type2, 0x93, "Align &Middle", "Align the middle of all the selected elements", Shortcut.None, 0x2f);
                this.AddMenuItem(this.menuItemTable2, type2, 0x94, "Align &Bottom Edges", "Align the bottom edges of the selected elements", Shortcut.None, 0x30);
                this.AddMenuItem(this.menuItemTable2, type2, 0x95, "Make Same &Width", "Make all the selected elements the same width", Shortcut.None, 0x31);
                this.AddMenuItem(this.menuItemTable2, type2, 150, "Make Same &Height", "Make all the selected elements the same height", Shortcut.None, 50);
                this.AddMenuItem(this.menuItemTable2, type2, 0x97, "Make Same &Size", "Make all the selected elements elements the same size", Shortcut.None, 0x33);
                this.AddMenuItem(this.menuItemTable2, type2, 0x8d, "Send &Forward", "Bring the selected element forward in z-order", Shortcut.None, 0x29);
                this.AddMenuItem(this.menuItemTable2, type2, 0x8e, "Send Bac&kward", "Send the selected element backward in z-order", Shortcut.None, 0x2a);
                this.AddMenuItem(this.menuItemTable2, type2, 0x98, "L&ocked", "Lock or unlock the position of the selected element", Shortcut.None, 0x27);
                this.AddMenuItem(this.menuItemTable2, type2, 200, "Insert &HyperLink...", "Insert an hyperLink into the current file", Shortcut.None, 0x1a);
                this.AddMenuItem(this.menuItemTable2, type2, 0xc9, "&Remove HyperLink", "Remove the selected hyperLink", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 220, "Insert &Table...", "Insert a table into the current file", Shortcut.None, 0x34);
                this.AddMenuItem(this.menuItemTable2, type2, 0xca, "Wrap in &Span", "Wrap the current selection with <span> tag", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0xcb, "Wrap in &Div", "Wrap the current selection with <div> tag", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable2, type2, 0xdd, "&Insert Table Row", "Insert a table row after the currently selected table cell", Shortcut.None, 0x44);
                this.AddMenuItem(this.menuItemTable2, type2, 0xde, "I&nsert Table Column", "Insert a table column after the currently selected table cell", Shortcut.None, 0x45);
                this.AddMenuItem(this.menuItemTable2, type2, 0xdf, "&Delete Table Row", "Delete the currently selected table row", Shortcut.None, 70);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe0, "D&elete Table Column", "Delete the currently selected table column", Shortcut.None, 0x47);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe1, "&Merge Cell Left", "Merge the currently selected table cell with the cell to the left", Shortcut.None, 0x48);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe2, "Me&rge Cell Right", "Merge the currently selected table cell with the cell to the right", Shortcut.None, 0x49);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe3, "Mer&ge Cell Up", "Merge the currently selected table cell with the cell above", Shortcut.None, 0x4a);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe4, "Merge &Cell Down", "Merge the currently selected table cell with the cell below", Shortcut.None, 0x4b);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe5, "&Split Cell Horizontally", "Split the currently selected table cell horizontally", Shortcut.None, 0x4c);
                this.AddMenuItem(this.menuItemTable2, type2, 0xe7, "S&plit Cell Vertically", "Split the currently selected table cell vertically", Shortcut.None, 0x4d);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x145, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x146, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x147, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x144, "&Preferences...", "Change preferences and options", Shortcut.None, 0x3f);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 320, "&Run Add-in...", "Run a particular add-in", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x141, "&Organize Add-ins...", "Organize the add-in list", Shortcut.None, 0x35);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.menuItemTable1, commandGroup, num + 300, "", "Run this add-in", (Shortcut) (0x30070 + num), -1);
                }
                this.AddMenuItem(this.menuItemTable1, commandGroup, 400, "&Cascade", "Arrange the current windows diagonally starting from the left top", Shortcut.None, 9);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x191, "Tile &Horizontal", "Tile the current windows horizontally", Shortcut.None, 10);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x192, "Tile &Vertical", "Tile the current windows vertically", Shortcut.None, 11);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x193, "Close &All", "Close all open windows", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 600, "&Help Topics", "View help topics", Shortcut.F1, 0x1b);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x25e, "Send &Feedback...", "Submit suggestions, bug reports or general feedback", Shortcut.None, 0x39);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x25a, "Application &Information...", "Information about this application and process", Shortcut.None, 0x38);
                this.AddMenuItem(this.menuItemTable1, commandGroup, 0x259, "About Microsoft ASP.NET &Web Matrix...", "About this application", Shortcut.None, -1);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.menuItemTable1, commandGroup, num + 610, "", "Browse this help URL", Shortcut.None, 0x1c);
                }
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x2c7, "Add New File...", "", Shortcut.None, 1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x2c8, "Add New Folder...", "", Shortcut.None, 0x3d, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x2c9, "Delete", "", Shortcut.None, 0x3b, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 710, "Open Files", "", Shortcut.None, 0x3a, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x2ca, "Refresh", "", Shortcut.None, 60, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 10, "New Project...", "", Shortcut.None, 0x37, true);
                for (num = 0; num < 8; num++)
                {
                    this.AddMenuItem(this.menuItemTable3, commandGroup, num + 720, "", "", Shortcut.None, -1, true);
                }
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x145, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x146, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x147, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x148, "Rename", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x149, "Remove", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x14b, "Sort by Name", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 330, "Reset Toolbox", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.menuItemTable4, type2, 260, "&Edit Tag", "Edit the source of the selected tag", Shortcut.CtrlT, 0x36);
                this.AddMenuItem(this.menuItemTable4, type2, 240, "Edit Temp&lates...", "Edit the templates of the selected control", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable4, commandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.menuItemTable4, commandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.menuItemTable4, commandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);
                this.AddMenuItem(this.menuItemTable5, type2, 260, "&Edit Tag", "Edit the source of the selected tag", Shortcut.CtrlT, 0x36);
                this.AddMenuItem(this.menuItemTable5, commandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.menuItemTable5, commandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.menuItemTable5, commandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);
                this.AddMenuItem(this.menuItemTable5, type2, 0xdd, "&Insert Table Row", "Insert a table row after the currently selected table cell", Shortcut.None, 0x44);
                this.AddMenuItem(this.menuItemTable5, type2, 0xde, "I&nsert Table Column", "Insert a table column after the currently selected table cell", Shortcut.None, 0x45);
                this.AddMenuItem(this.menuItemTable5, type2, 0xdf, "&Delete Table Row", "Delete the currently selected table row", Shortcut.None, 70);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe0, "D&elete Table Column", "Delete the currently selected table column", Shortcut.None, 0x47);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe1, "&Merge Cell Left", "Merge the currently selected table cell with the cell to the left", Shortcut.None, 0x48);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe2, "Me&rge Cell Right", "Merge the currently selected table cell with the cell to the right", Shortcut.None, 0x49);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe3, "Mer&ge Cell Up", "Merge the currently selected table cell with the cell above", Shortcut.None, 0x4a);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe4, "Merge &Cell Down", "Merge the currently selected table cell with the cell below", Shortcut.None, 0x4b);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe5, "Spli&t Cell Horizontally", "Split the currently selected table cell horizontally", Shortcut.None, 0x4c);
                this.AddMenuItem(this.menuItemTable5, type2, 0xe7, "S&plit Cell Vertically", "Split the currently selected table cell vertically", Shortcut.None, 0x4d);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 120, "A&dd Snippet", "Copy the selected text as a snippet", Shortcut.None, -1);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.menuItemTable3, commandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);

                    this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 1, "New File", 1);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 2, "Open Files", 2);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 3, "Save File", 3);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 10, "New Project", 0x37);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 6, "Print File", 0x1f);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x66, "Cut", 6);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x67, "Copy", 7);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x68, "Paste", 8);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 100, "Undo", 4);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x65, "Redo", 5);
                this.AddToolBarComboBoxButton(this.toolBarButtonTable1, commandGroup, 110, 0x6f, "Find", 20, ComboBoxStyle.DropDown, null, "Enter a search string");
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 210, "Start", 0x3e);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0xd6, "Toggle Toolbox", 0x43);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 1, "Toggle Glyphs", 0x26);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 2, "Toggle Design Borders", 0x25);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 3, "Toggle Design Grid", 0x23);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 4, "Toggle Snap to Grid", 0x24);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 600, "Help Topics", 0x1b);
                string[] items = new string[] { "Normal", "Formatted", "Heading 1", "Heading 2", "Heading 3", "Heading 4", "Heading 5", "Heading 6", "Paragraph" };
                string[] strArray2 = new string[] { "1", "2", "3", "4", "5", "6", "7" };
                this.AddToolBarComboBoxButton(this.toolBarButtonTable2, type2, 120, "Block Format", 8, ComboBoxStyle.DropDownList, items);
                this.AddToolBarFontComboBoxButton(this.toolBarButtonTable2, type2, 0x79, "Font", 20);
                this.AddToolBarComboBoxButton(this.toolBarButtonTable2, type2, 0x7a, "Font Size", 2, ComboBoxStyle.DropDownList, strArray2);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x6a, "Foreground Color", 0x20);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x6b, "Background Color", 0x21);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 100, "Bold", 12);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x65, "Italic", 13);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x66, "Underline", 14);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x6c, "Align Left", 0x11);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 110, "Align Center", 0x12);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x6d, "Align Right", 0x13);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x6f, "Ordered List", 20);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x70, "Unordered List", 0x15);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x72, "Unindent", 0x16);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 0x71, "Indent", 0x17);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 260, "Edit Tag", 0x36);
                this.AddToolBarButton(this.toolBarButtonTable2, type2, 140, "Absolute Position", 0x19);

}
