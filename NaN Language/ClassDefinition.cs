using System.Windows.Forms;

namespace NaN_Language
{
    public enum 命名
    {
        ㄅ, ㄆ, ㄇ, ㄈ, ㄉ, ㄊ, ㄋ, ㄌ, ㄍ, ㄎ, ㄏ, ㄐ, ㄒ,
        ㄓ, ㄔ, ㄕ, ㄖ, ㄗ, ㄘ, ㄙ, ㄧ, ㄨ, ㄩ,
        ㄚ, ㄛ, ㄜ, ㄝ, ㄞ, ㄟ, ㄠ, ㄡ, ㄢ, ㄣ, ㄤ, ㄥ, ㄦ,
        一, 二, 三, 四, 五, 六, 七, 八, 九, 十,
        之, 的, 中, _, 工, 从
    };
    public enum 變數當中的
    {
        空值, 整數, 小數, 字串
    };
    public enum 元件當中的
    {
        視窗, 按鍵, 字箱, 標籤, 事件, 變數
    };
    public enum 符號當中的
    {
        空值, 加, 減, 乘, 除以, 等於, 大於, 小於, 全等於
    };
    public class 工具箱裡的
    {
        public 變數當中的 變數是;
        public 元件當中的 元件是;
        public Control 控制項是;
        public 符號當中的 符號是;
        public int 第;
        public string 名稱是;
        public string 內容是;
        public long 整數數值為;
        public double 小數數值為;
        public bool 這個工具有沒有被觸發;
        public 工具箱裡的 這個符號牽涉的第一個東西;
        public 工具箱裡的 這個符號牽涉的第二個東西;
    }
    public class 新視窗 : Form
    {
        public Button 按鍵;
        public int 按鍵次序 = 1;
        public TextBox 字箱;
        public int 字箱次序 = 1;
        public Label 標籤;
        public int 標籤次序 = 1;
    }

}