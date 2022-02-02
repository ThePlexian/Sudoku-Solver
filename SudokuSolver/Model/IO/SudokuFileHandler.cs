using System.Xml;

using SudokuSolver.Model.Data;

namespace SudokuSolver.Model.IO
{
    internal static class SudokuFileHandler
    {
        public static LoadingProcessResult LoadTxt(string path, out List<Sudoku> sudokus)
        {
            sudokus = new List<Sudoku>();


            //Start reading
            string[]? text = File.ReadAllLines(path);
            int counter = 1;

            foreach (string? line in text) {
                //Read
                var s = new Sudoku("Sudoku No. " + counter);
                if (!ReadLine(line, ref s)) {
                    return LoadingProcessResult.InvalidFileContent;
                }

                //Add if successful
                sudokus.Add(s);
                counter++;
            }

            return LoadingProcessResult.Success;
        }

        public static LoadingProcessResult LoadXml(string path, out List<Sudoku> sudokus)
        {
            sudokus = new List<Sudoku>();

            //Start reading
            try {
                var doc = new XmlDocument();
                doc.Load(path);

                var xmlNodeList = doc.SelectNodes("Sudokus");
                var snodes = xmlNodeList?[0].SelectNodes("Sudoku");

                if (snodes != null) {
                    foreach (XmlNode snode in snodes) {
                        var xmlNode = snode.ChildNodes.Item(0);
                        if (xmlNode == null) {
                            continue;
                        }

                        var s = new Sudoku { Name = xmlNode.InnerText };

                        //Read children
                        var item1 = snode.ChildNodes.Item(1);
                        var item2 = snode.ChildNodes.Item(2);
                        if (item1 == null || item2 == null) {
                            return LoadingProcessResult.InvalidFileContent;
                        }

                        string? cells = item1.InnerText;
                        string? preset = item2.InnerText;

                        //Adjust cells
                        bool validline = ReadLine(cells, ref s);
                        for (int i = 0; i <= 80; i++) {
                            s.SetPresetValue(i, preset[i] == 'p');
                        }

                        sudokus.Add(validline ? s : null);
                    }
                }
            }
            catch { return LoadingProcessResult.InvalidFileContent; }

            return LoadingProcessResult.Success;
        }

        public static SavingProcessResult SaveTxt(Sudoku s, string path, FileAccess fa)
        {

            //Do not overwrite
            bool fileexists = File.Exists(path);
            bool append = fa == FileAccess.CreateOrAppend;

            if (fileexists && fa == FileAccess.CreateOnly) {
                return SavingProcessResult.FileAlreadyExists;
            }


            //Saving
            try {
                using var sw = new StreamWriter(path, append);
                if (append) {
                    sw.WriteLine();
                }

                sw.Write(CellsToString(s));
            }
            catch { return SavingProcessResult.UnauthorizedAccess; }

            return SavingProcessResult.Success;
        }

        public static SavingProcessResult SaveXml(Sudoku s, string path, FileAccess fa)
        {

            //Do not overwrite
            bool fileexists = File.Exists(path);
            bool append = fa == FileAccess.CreateOrAppend;

            if (fileexists && fa == FileAccess.CreateOnly) {
                return SavingProcessResult.FileAlreadyExists;
            }


            //Saving
            try {
                var doc = new XmlDocument();
                XmlElement body = null;
                if (!append || !fileexists) {
                    //Create new header
                    //Declaration
                    var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    var root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    //Body
                    body = doc.CreateElement(string.Empty, "Sudokus", string.Empty);
                    doc.AppendChild(body);
                }
                else {
                    //Just load the existing one
                    doc.Load(path);
                    var xmlNodeList = doc.SelectNodes("Sudokus");
                    if (xmlNodeList != null) {
                        body = xmlNodeList[0] as XmlElement;
                    }
                }


                //Sudoku
                if (body != null) {
                    body.AppendChild(CreateSudokuElement(s, doc));
                }

                doc.Save(path);
            }
            catch { return SavingProcessResult.UnauthorizedAccess; }

            return SavingProcessResult.Success;
        }




       
        // **** PRIVATE ****

        private static bool ReadLine(string text, ref Sudoku sudoku)
        {
            //Preset
            var oldsudoku = sudoku;
            sudoku = new Sudoku(sudoku.Name);

            text = text.Replace(".", "0");                  //Replace, cause both dots and zeros are valid
            text = text.Replace(" ", "");                   //Replace, cause spaces are allowed as seperator, but are not necessary
            text = text.Replace(Environment.NewLine, "");   //Linefeeds are allowed as seperator, but are not necessary
            text = text.Replace("\n", "");                  //Linefeeds are allowed as seperator, but are not necessary

            //Length of text invalid
            if (text.Length != 81) {
                return false;
            }

            //Try to set cells
            int i = 0;
            foreach (char c in text) {
                //No number
                if (!int.TryParse(c.ToString(), out int n)) {
                    return false;
                }

                if (n != 0) {
                    if (!sudoku.SetValue(i, n)) {
                        return false;
                    }
                }

                sudoku.SetPresetValue(i, n != 0);
                i++;
            }


            //Add eventhandlers afterwards
            //sudoku.CellChanged += oldsudoku.CellChanged;
            //sudoku.SolvingCompleted += oldsudoku.SolvingCompleted;

            return true;
        }

        private static XmlElement CreateSudokuElement(Sudoku s, XmlDocument doc)
        {
            //New sudoku
            var parent = doc.CreateElement(string.Empty, "Sudoku", string.Empty);
            var date = doc.CreateAttribute("Date");
            date.Value = DateTime.Now.ToString();

            //Name
            var name = doc.CreateElement(string.Empty, "Name", string.Empty);
            name.AppendChild(doc.CreateTextNode(s.Name));

            //Cells
            var cells = doc.CreateElement(string.Empty, "Cells", string.Empty);
            cells.AppendChild(doc.CreateTextNode(CellsToString(s)));

            //Preset
            var presets = doc.CreateElement(string.Empty, "Preset", string.Empty);
            presets.AppendChild(doc.CreateTextNode(string.Join("", s.GetCellsIterated().ToList().Select(c => c.IsPreset ? "p" : "."))));

            parent.SetAttributeNode(date);
            parent.AppendChild(name);
            parent.AppendChild(cells);
            parent.AppendChild(presets);

            return parent;
        }

        private static string CellsToString(Sudoku s)
        {
            return string.Join("", s.GetCellsIterated().Select(c => c.Number == 0 ? "." : c.ToString()));
        }


    }
}
