using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Forms;
using System.IO;

namespace NaN_Language
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        static Timer 計時器 = new Timer();
        public List<工具箱裡的> 工具的 = new List<工具箱裡的>();
        變數當中的 數字種類是 = new 變數當中的();
        string file_name = "新方案"; // 檔案名稱
        public string CODES; //紀錄指令字串
        private TextRange textRange;
        private string[] 在所有指令的 = new string[1];
        public List<Node> NodeList { get; set; } //節點資料
        OpenFileDialog dialog = new OpenFileDialog(); //選擇檔案目錄對話框
        public MainWindow()
        {
            InitializeComponent();
            NodeList = GetNodeList();
            物件列表.ItemsSource = NodeList;
            程式碼.AppendText("難乎哉？\n");
            程式碼.AppendText("甚難矣！");
            計時器事件();
        }
        新視窗 視窗 = new 新視窗();
        public enum NodeType
        {
            RootNode,//根
            LeafNode,//葉
            StructureNode//結構
        }
        public class Node
        {
            public Node()
            {
                NodeId = Guid.NewGuid().ToString();
                IsDeleted = false;
                Nodes = new List<Node>();
            }

            /// <summary>
            /// 節點ID
            /// </summary>
            public string NodeId { get; set; }

            /// <summary>
            ///  名稱
            /// </summary>
            public string NodeName { get; set; }

            /// <summary>
            /// 攜帶的内容
            /// </summary>
            public string NodeContent { get; set; }

            /// <summary>
            /// 被删除
            /// </summary>
            public bool IsDeleted { get; set; }

            /// <summary>
            /// 類型
            /// </summary>
            public NodeType NodeType { get; set; }

            /// <summary>
            /// 子節點集合
            /// </summary>
            public List<Node> Nodes { get; set; }
        }
        private List<Node> GetNodeList()
        {
            int num = file_name.Count() , last, first = 0;
            string refill = "";
            for (last = num - 1; last > 0; last--)
            {
                if (file_name[last].Equals('\\'))
                {
                    first = last;
                    break;
                }
            }
            if (first == 0)
                last = 0;
            else
                last++;
            for (first = last; first < num; first++)
            {
                refill += file_name[first];
            }

            Node 二級節點 = new Node();
            二級節點.NodeName = "窗頂 [" + 視窗.Text + "]";
            二級節點.NodeContent = "二級節點";
            二級節點.NodeType = NodeType.StructureNode;
            二級節點.Nodes = new List<Node>() { };

            for (int 視窗元件 = 0; 視窗元件 < 視窗.Controls.Count; 視窗元件++)
            {
                Node 三級節點 = new Node
                {
                    NodeName = 視窗.Controls[視窗元件].Name + "[" + 視窗.Controls[視窗元件].Text + "]",
                    NodeContent = "三級節點",
                    NodeType = NodeType.StructureNode,
                    Nodes = new List<Node>() { }
                };
                二級節點.Nodes.Add(三級節點);
            }

            Node 一級節點 = new Node
            {
                NodeName = "窗口",
                NodeContent = "一級節點",
                NodeType = NodeType.StructureNode,
                Nodes = new List<Node>() { 二級節點 }
            };
            return new List<Node>()
            {
                new Node(){NodeName="方案[ "+refill+" ]",NodeContent="視窗根目錄",NodeType=NodeType.RootNode,Nodes=new List<Node>(){一級節點}}
            };
        }

        private void 編譯_Click(object sender, RoutedEventArgs e)
        {
            NodeList.Clear();
            NodeList = GetNodeList();
            物件列表.ItemsSource = NodeList;
            textRange = new TextRange(程式碼.Document.ContentStart, 程式碼.Document.ContentEnd);
            CODES = textRange.Text;
            在所有指令的 = CODES.Split(' ', '[', ']', '\n', '\r');
            StringBuilder str = new StringBuilder("編譯中...\n");
            除錯訊息.Text += str.ToString();
            視窗.Close();
            視窗.Controls.Clear();
        }
        private void 運行_Click(object sender, RoutedEventArgs e)
        {
            編譯主程式();
        }

        private void 讀檔_Click(object sender, RoutedEventArgs e)
        {
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "難程式碼(*.nan)|*.nan|(*.NAN)|*.NAN";
            string line = null;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file_name = dialog.FileName;
            }
            if (file_name != "新方案")
            {
            StreamReader sr = new StreamReader(file_name);
            while (!sr.EndOfStream)
            {               // 每次讀取一行，直到檔尾
                line += sr.ReadLine() + "\n";            // 讀取文字到 line 變數
            }
            sr.Close();						// 關閉串流
            this.程式碼.Document.Blocks.Clear();
            程式碼.AppendText(line);
            }
        }

        private void 存檔_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(file_name))
            {
                System.Windows.MessageBox.Show("竟然還沒指定檔案路徑", "好玩嗎？", MessageBoxButton.OK);
                SaveAsFileFunc();
                if (file_name != null)
                SaveFileFunc();
            }
            else
                SaveFileFunc();
            
        }
        
        private void 另存_Click(object sender, RoutedEventArgs e)
        {
            SaveAsFileFunc();
            if (file_name != null)
                SaveFileFunc();
        }
        /// <summary>
        /// 存檔流程控制
        /// </summary>
        private void SaveFileFunc()
        {
            if (file_name != "新方案")
            {
                string Attachment = file_name.Substring(file_name.LastIndexOf('.'), file_name.Length - file_name.LastIndexOf('.')); //取得附檔名
                if (Attachment == ".nan" || Attachment == ".NAN")
                {
                    using (FileStream stream = File.OpenWrite(file_name))
                    {
                        TextRange documentTextRange = new TextRange(程式碼.Document.ContentStart, 程式碼.Document.ContentEnd);
                        string dataFormat = System.Windows.DataFormats.Text;
                        string ext = System.IO.Path.GetExtension(file_name);
                        documentTextRange.Save(stream, dataFormat);
                    }
                }
                else System.Windows.MessageBox.Show("檔名弄錯了你知道嗎？", "幹什麼", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// 另存新檔流程控制
        /// </summary>
        private void SaveAsFileFunc()
        {
            Microsoft.Win32.SaveFileDialog SaveDialog = new Microsoft.Win32.SaveFileDialog();   //創造讀取對話方塊
            SaveDialog.Filter = "難 (*.nan)|*.nan|(*.NAN)|*.NAN";    //設定過濾附檔名
            if (SaveDialog.ShowDialog() == true)
                file_name = SaveDialog.FileName;
        }

        List<TextRange> FindWordFromPosition(TextPointer position, string word)
        {
            List<TextRange> matchingText = new List<TextRange>();
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //帶有內容的文本
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    //查找關鍵字在這文本中的位置
                    int indexInRun = textRun.IndexOf(word);
                    int indexHistory = 0;
                    while (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun + indexHistory);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        matchingText.Add(new TextRange(start, end));

                        indexHistory = indexHistory + indexInRun + word.Length;
                        textRun = textRun.Substring(indexInRun + word.Length);//去掉已經採集過的內容
                        indexInRun = textRun.IndexOf(word);//重新判斷新的字符串是否還有關鍵字
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return matchingText;
        }

        private void 修改程式碼(object sender, TextChangedEventArgs e)
        {
            if( 程式碼.Document.Blocks.Count > 6)
            {
                List<TextRange> Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "命名為");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                }
                Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "定量為");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                }
                Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "建立");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                }
                Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "字箱");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
                }
                Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "標籤");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
                }
                Text_Ranges = FindWordFromPosition(程式碼.Document.ContentStart, "按鍵");
                foreach (TextRange range in Text_Ranges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
                }
            }
        }
        private void 計時器事件()
        {
            計時器.Interval = 1000;
            計時器.Tick += new EventHandler(離開畫面);
            計時器.Enabled = true;
        }

        private void 離開畫面(object sender, EventArgs e)
        {
            for (int 每一個 = 1; 每一個 < 工具的.Count; 每一個++)
            {
               
                元件當中的 名稱 = 工具的[每一個].元件是;
                if (名稱 == 元件當中的.字箱 || 名稱 == 元件當中的.標籤)
                {
                    if (數字種類是 == 變數當中的.整數)
                    {
                        if (long.TryParse(工具的[每一個].內容是, out long 整數輸出值))
                        {
                            工具的[每一個].整數數值為 = 整數輸出值;
                        }
                    }
                    else if (數字種類是 == 變數當中的.小數)
                    {
                        if (double.TryParse(工具的[每一個].內容是, out double 小數輸出值))
                        {
                            工具的[每一個].小數數值為 = 小數輸出值;
                        }
                    }
                }
            }
            重新計算();
        }

        private void 觸發_Click(object sender, RoutedEventArgs e)
        {
            視窗.Close();
        }
        private void 新按鍵_Click(object sender, EventArgs e)
        {
            int 控制項數目 = 工具的.Count;
            for (int 序號 = 0; 序號 < 控制項數目; 序號++)
            {
                元件當中的 名稱 = 工具的[序號].元件是;
                System.Windows.Forms.Button 按鍵 = sender as System.Windows.Forms.Button;
                string sender_name = 按鍵.Name;
                if (名稱 == 元件當中的.按鍵)
                {
                    if (sender_name == 工具的[序號].名稱是)
                    {
                        工具的[序號].這個工具有沒有被觸發 = true;
                        重新計算();
                        break;
                    }
                }
            }
            計時器事件();
        }

        private void 重新計算()
        {
            //將每個工具的最新內容輸入至「內容是】
            for (int 序數 = 0; 序數 < 工具的.Count; 序數++)
            {
                if (工具的[序數].控制項是 != null)
                {
                    工具的[序數].內容是 = 工具的[序數].控制項是.Text;
                }
            }
            符號當中的 那一個符號是 = new 符號當中的();
            int 序號 = 0;
            for (int 順序 = 0; 順序 < 工具的.Count; 順序++)
            {
                if (工具的[順序].這個工具有沒有被觸發)
                {
                    序號 = 工具的[順序].第;
                    break;
                }
            }
            if (序號 > 0)
            {
                那一個符號是 = 工具的[序號].符號是;
            }
            if (那一個符號是 == 符號當中的.等於)
            {
                if (工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西.符號是 != 符號當中的.空值)
                {
                    工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西 = 我才是真的在計算(工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西);
                    if (工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西.變數是 == 變數當中的.整數)
                    {
                        工具的[序號].整數數值為 = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西.整數數值為;
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.整數數值為 = 工具的[序號].整數數值為;
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.內容是 = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.整數數值為.ToString();
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.控制項是.Text = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.內容是;
                    }
                    else if (工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西.變數是 == 變數當中的.小數)
                    {
                        工具的[序號].小數數值為 = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第一個東西.小數數值為;
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.小數數值為 = 工具的[序號].小數數值為;
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.內容是 = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.小數數值為.ToString();
                        工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.控制項是.Text = 工具的[序號].這個符號牽涉的第一個東西.這個符號牽涉的第二個東西.內容是;
                    }
                }
                else
                {
                    工具的[序號].內容是 = "數值運算有問題";
                }
            }
            if (序號 > 0)
            {
                工具的[序號].這個工具有沒有被觸發 = false;
            }
            視窗.Refresh();
        }
        private 工具箱裡的 我才是真的在計算(工具箱裡的 的那個東西)
        {
            符號當中的 那一個符號是 = 的那個東西.符號是;
            變數當中的 那一個數字是 = 的那個東西.變數是;
            if (那一個符號是 == 符號當中的.加)
            {
                if (那一個數字是 == 變數當中的.整數)
                {
                    的那個東西.整數數值為 = 的那個東西.這個符號牽涉的第一個東西.整數數值為 + 的那個東西.這個符號牽涉的第二個東西.整數數值為;
                }
                else if (那一個數字是 == 變數當中的.小數)
                {
                    的那個東西.小數數值為 = 的那個東西.這個符號牽涉的第一個東西.小數數值為 + 的那個東西.這個符號牽涉的第二個東西.小數數值為;
                }
            }
            else if (那一個符號是 == 符號當中的.減)
            {
                if (那一個數字是 == 變數當中的.整數)
                {
                    的那個東西.整數數值為 = 的那個東西.這個符號牽涉的第一個東西.整數數值為 - 的那個東西.這個符號牽涉的第二個東西.整數數值為;
                }
                else if (那一個數字是 == 變數當中的.小數)
                {
                    的那個東西.小數數值為 = 的那個東西.這個符號牽涉的第一個東西.小數數值為 - 的那個東西.這個符號牽涉的第二個東西.小數數值為;
                }
            }
            else if (那一個符號是 == 符號當中的.乘)
            {
                if (那一個數字是 == 變數當中的.整數)
                {
                    的那個東西.整數數值為 = 的那個東西.這個符號牽涉的第一個東西.整數數值為 * 的那個東西.這個符號牽涉的第二個東西.整數數值為;
                }
                else if (那一個數字是 == 變數當中的.小數)
                {
                    的那個東西.小數數值為 = 的那個東西.這個符號牽涉的第一個東西.小數數值為 * 的那個東西.這個符號牽涉的第二個東西.小數數值為;
                }
            }
            else if (那一個符號是 == 符號當中的.除以)
            {
                if (那一個數字是 == 變數當中的.整數)
                {
                    if (的那個東西.這個符號牽涉的第二個東西.整數數值為 != 0)
                    {
                        的那個東西.整數數值為 = 的那個東西.這個符號牽涉的第一個東西.整數數值為 / 的那個東西.這個符號牽涉的第二個東西.整數數值為;
                    }
                }
                else if (那一個數字是 == 變數當中的.小數)
                {
                    if (Math.Abs(的那個東西.這個符號牽涉的第二個東西.小數數值為) > 1e-10)
                    {
                        的那個東西.小數數值為 = 的那個東西.這個符號牽涉的第一個東西.小數數值為 / 的那個東西.這個符號牽涉的第二個東西.小數數值為;
                    }
                }
            }
            return 的那個東西;
        }

        public void 編譯主程式()
        {

            工具的.Clear();
            float 字體大小 = 10f;
            char[] 字體名稱 = { '細', '明', '體' };
            System.Drawing.FontStyle 字體樣式 = System.Drawing.FontStyle.Regular;
            if (視窗.IsDisposed)
            {
                視窗 = new 新視窗();
            }
            else
            {
                視窗.Refresh();
            }

            視窗.Text = "新視窗";
            視窗.Font = new Font(new string(字體名稱), 字體大小, 字體樣式);
            工具箱裡的 初始元件視窗 = new 工具箱裡的();
            初始元件視窗.元件是 = 元件當中的.視窗;
            初始元件視窗.內容是 = 視窗.Text;
            初始元件視窗.名稱是 = 視窗.Text;
            初始元件視窗.第 = 0;
            初始元件視窗.控制項是 = 視窗;
            工具的.Add(初始元件視窗);
            int 這個位置 = 0; //讀取tokens陣列的第幾項
            int button_x = 0, button_y = 0;
            int textbox_x = 0, textbox_y = 0;
            int label_x = 0, label_y = 0;
            foreach (string 若這個指令 in 在所有指令的)
            {
                if (!string.IsNullOrEmpty(CODES.Trim()))
                {
                    if (若這個指令 == "難乎哉？")
                    {
                        StringBuilder str = new StringBuilder("編譯成功\n開啟程式\n");
                        除錯訊息.Text += str.ToString();
                    }
                    else if (若這個指令 == "建立")
                    {
                        工具箱裡的 新工具 = new 工具箱裡的();
                        if (在所有指令的[這個位置 + 1] == "按鍵")
                        {
                            視窗.按鍵 = new System.Windows.Forms.Button
                            {
                                Name = $"按鍵{視窗.按鍵次序}"
                            };
                            視窗.按鍵.Text = 視窗.按鍵.Name;
                            視窗.按鍵.Font = new Font(new string(字體名稱), 字體大小, 字體樣式);
                            視窗.按鍵.Enabled = true;
                            視窗.按鍵次序 += 1;
                            視窗.按鍵.Location = new System.Drawing.Point(0, 0);
                            視窗.按鍵.Click += new EventHandler(新按鍵_Click);
                            視窗.Controls.Add(視窗.按鍵);
                            新工具.元件是 = 元件當中的.按鍵;
                            新工具.內容是 = "";
                            新工具.名稱是 = 視窗.按鍵.Name;
                            新工具.變數是 = 數字種類是;
                            新工具.控制項是 = 視窗.按鍵;
                            工具的.Add(新工具);
                            NodeList.Clear();
                            NodeList = GetNodeList();
                            物件列表.ItemsSource = NodeList;
                        }
                        else if (在所有指令的[這個位置 + 1] == "字箱")
                        {
                            視窗.字箱 = new System.Windows.Forms.TextBox
                            {
                                Name = $"字箱{視窗.字箱次序}"
                            };
                            視窗.字箱.Text = 視窗.字箱.Name;
                            視窗.字箱.Font = new Font(new string(字體名稱), 字體大小, 字體樣式);
                            視窗.字箱.Enabled = true;
                            視窗.字箱次序 += 1;
                            視窗.字箱.Location = new System.Drawing.Point(0, 0);
                            視窗.Controls.Add(視窗.字箱);
                            新工具.元件是 = 元件當中的.字箱;
                            新工具.內容是 = 視窗.字箱.Text;
                            新工具.名稱是 = 視窗.字箱.Name;
                            新工具.變數是 = 數字種類是;
                            新工具.控制項是 = 視窗.字箱;
                            工具的.Add(新工具);
                            NodeList.Clear();
                            NodeList = GetNodeList();
                            物件列表.ItemsSource = NodeList;
                        }
                        else if (在所有指令的[這個位置 + 1] == "標籤")
                        {
                            視窗.標籤 = new System.Windows.Forms.Label
                            {
                                Name = $"標籤{視窗.標籤次序}"
                            };
                            視窗.標籤.Text = 視窗.標籤.Name;
                            視窗.標籤.Font = new Font(new string(字體名稱), 字體大小, 字體樣式);
                            視窗.標籤.Enabled = true;
                            視窗.標籤次序 += 1;
                            視窗.標籤.Location = new System.Drawing.Point(0, 0);
                            視窗.Controls.Add(視窗.標籤);
                            新工具.元件是 = 元件當中的.標籤;
                            新工具.內容是 = 視窗.標籤.Text;
                            新工具.名稱是 = 視窗.標籤.Name;
                            新工具.變數是 = 數字種類是;
                            新工具.控制項是 = 視窗.標籤;
                            工具的.Add(新工具);
                            NodeList.Clear();
                            NodeList = GetNodeList();
                            物件列表.ItemsSource = NodeList;
                        }

                    }
                    else if (若這個指令 == "命名為")
                    {
                        if (在所有指令的[這個位置 - 1] == "窗頂")
                        {
                            視窗.Text = 在所有指令的[這個位置 + 1];
                            NodeList.Clear();
                            NodeList = GetNodeList();
                            物件列表.ItemsSource = NodeList;
                        }
                        else if (在所有指令的[這個位置 - 1] == "字體")
                        {
                            字體名稱 = 在所有指令的[這個位置 + 1].ToArray();
                        }
                        else if (在所有指令的[這個位置 - 1] == "字體樣式")
                        {
                            string Style = 在所有指令的[這個位置 + 1];
                            if (Style == "標準")
                            { 字體樣式 = System.Drawing.FontStyle.Regular; }
                            else if (Style == "粗體")
                            { 字體樣式 = System.Drawing.FontStyle.Bold; }
                            else if (Style == "斜體")
                            { 字體樣式 = System.Drawing.FontStyle.Italic; }
                            else if (Style == "底線")
                            { 字體樣式 = System.Drawing.FontStyle.Underline; }
                            else if (Style == "中線")
                            { 字體樣式 = System.Drawing.FontStyle.Strikeout; }
                        }
                        else if (在所有指令的[這個位置 - 1] == "按鍵")
                        {
                            視窗.按鍵.Name = 在所有指令的[這個位置 + 1];
                        }
                        else if (在所有指令的[這個位置 - 1] == "字箱")
                        {
                            視窗.字箱.Name = 在所有指令的[這個位置 + 1];
                        }
                        else if (在所有指令的[這個位置 - 1] == "標籤")
                        {
                            視窗.標籤.Name = 在所有指令的[這個位置 + 1];
                        }
                        //將每個工具的最新內容輸入至「內容是】
                        for (int 序數 = 0; 序數 < 工具的.Count; 序數++)
                        {
                            工具的[序數].內容是 = 工具的[序數].控制項是.Text;
                        }
                        NodeList.Clear();
                        NodeList = GetNodeList();
                        物件列表.ItemsSource = NodeList;
                    }

                    else if (若這個指令 == "定量為")
                    {
                        if (在所有指令的[這個位置 - 1] == "字體大小")
                        {
                            字體大小 = Convert.ToSingle(在所有指令的[這個位置 + 1]);
                        }
                        else if (在所有指令的[這個位置 - 2] == "窗口")
                        {
                            if (在所有指令的[這個位置 - 1] == "的高")
                            {
                                視窗.Height = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的寬")
                            {
                                視窗.Width = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                        }
                        else if (在所有指令的[這個位置 - 2] == "按鍵")
                        {
                            if (在所有指令的[這個位置 - 1] == "的高")
                            {
                                視窗.按鍵.Height = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的寬")
                            {
                                視窗.按鍵.Width = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄨ")
                            {
                                button_x = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.按鍵.Location = new System.Drawing.Point(button_x, button_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄚ")
                            {
                                button_y = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.按鍵.Location = new System.Drawing.Point(button_x, button_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的值")
                            {
                                視窗.按鍵.Text = 在所有指令的[這個位置 + 1];
                            }
                        }
                        else if (在所有指令的[這個位置 - 2] == "字箱")
                        {
                            if (在所有指令的[這個位置 - 1] == "的高")
                            {
                                視窗.字箱.Height = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的寬")
                            {
                                視窗.字箱.Width = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄨ")
                            {
                                textbox_x = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.字箱.Location = new System.Drawing.Point(textbox_x, textbox_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄚ")
                            {
                                textbox_y = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.字箱.Location = new System.Drawing.Point(textbox_x, textbox_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的值")
                            {
                                視窗.字箱.Text = 在所有指令的[這個位置 + 1];
                            }
                        }
                        else if (在所有指令的[這個位置 - 2] == "標籤")
                        {
                            if (在所有指令的[這個位置 - 1] == "的高")
                            {
                                視窗.標籤.Height = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的寬")
                            {
                                視窗.標籤.Width = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄨ")
                            {
                                label_x = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.標籤.Location = new System.Drawing.Point(label_x, label_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的ㄚ")
                            {
                                label_y = Convert.ToInt32(在所有指令的[這個位置 + 1]);
                                視窗.標籤.Location = new System.Drawing.Point(label_x, label_y);
                            }
                            else if (在所有指令的[這個位置 - 1] == "的值")
                            {
                                視窗.標籤.Text = 在所有指令的[這個位置 + 1];
                            }
                        }
                        NodeList.Clear();
                        NodeList = GetNodeList();
                        物件列表.ItemsSource = NodeList;
                    }

                    else if (若這個指令 == "格式為")
                    {
                        if (在所有指令的[這個位置 + 1] == "整數")
                        {
                            數字種類是 = 變數當中的.整數;
                        }
                        else if (在所有指令的[這個位置 + 1] == "小數")
                        {
                            數字種類是 = 變數當中的.小數;
                        }
                        else if (在所有指令的[這個位置 + 1] == "字串")
                        {
                            數字種類是 = 變數當中的.字串;
                        }
                    }

                    else if (Enum.IsDefined(typeof(符號當中的), 若這個指令))
                    {
                        工具箱裡的 新工具 = new 工具箱裡的
                        {
                            元件是 = 元件當中的.事件,
                            內容是 = "",
                            名稱是 = 若這個指令,
                            符號是 = (符號當中的)Enum.Parse(typeof(符號當中的), 若這個指令),
                            變數是 = 數字種類是
                        };
                        工具的.Add(新工具);
                        if (新工具.符號是 == 符號當中的.等於 && 在所有指令的[這個位置 + 2] == "若按下")
                        {
                            string 上一個變數 = 在所有指令的[這個位置 - 2], 下一個變數 = 在所有指令的[這個位置 + 1], 下兩個變數 = 在所有指令的[這個位置 + 3];
                            //尋找上一個變數對應的元件
                            for (int 正確 = 1; 正確 < 工具的.Count; 正確++)
                            {
                                if (工具的[正確].名稱是 == 上一個變數)
                                {
                                    新工具.這個符號牽涉的第一個東西 = 工具的[正確];
                                    break;
                                }
                            }
                            //尋找下一個變數對應的元件
                            for (int 正確 = 1; 正確 < 工具的.Count; 正確++)
                            {
                                if (工具的[正確].名稱是 == 下一個變數)
                                {
                                    新工具.這個符號牽涉的第二個東西 = 工具的[正確];
                                    break;
                                }
                            }
                            for (int 正確 = 1; 正確 < 工具的.Count; 正確++)
                            {
                                if (工具的[正確].名稱是 == 下兩個變數)
                                {
                                    工具的[正確].符號是 = 新工具.符號是;
                                    工具的[正確].這個符號牽涉的第一個東西 = 新工具;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string 上一個變數 = 在所有指令的[這個位置 - 1], 下一個變數 = 在所有指令的[這個位置 + 1];
                            //尋找上一個變數對應的元件
                            for (int 正確 = 1; 正確 < 工具的.Count; 正確++)
                            {
                                if (工具的[正確].名稱是 == 上一個變數)
                                {
                                    新工具.這個符號牽涉的第一個東西 = 工具的[正確];
                                    break;
                                }
                            }
                            //尋找下一個變數對應的元件
                            for (int 正確 = 1; 正確 < 工具的.Count; 正確++)
                            {
                                if (工具的[正確].名稱是 == 下一個變數)
                                {
                                    新工具.這個符號牽涉的第二個東西 = 工具的[正確];
                                    break;
                                }
                            }
                        }
                    }

                    else if (若這個指令 == "甚難矣！")
                    {
                        break;
                    }
                }
                這個位置++;
            }

            int 控制項數目 = 工具的.Count;
            for (int 序號 = 0; 序號 < 控制項數目; 序號++)
            {
                工具的[序號].第 = 序號;
            }
            視窗.Show();

        }

        private void 失去焦點(object sender, RoutedEventArgs e)
        {
            計時器事件();
        }
    }

}
