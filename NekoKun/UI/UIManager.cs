using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class UIManager
    {
        /*
         * 那真是一段孤独得难以想象的旅程
         * 想像を絶するくらい 孤独な旅であるはずだ
         * 
         * 甚至连一个氢原子都接触不到
         * 本当の暗閣の中をただひたむきに
         * 1つの水素原子にさえ滅多に出会うことなく
         * 
         * 只是坚信着在那深渊中有世界的奥秘等着自己去探索
         * ただただ　深淵にあるはずと信じる 世界の秘密に近づきたいー心で
         * 
         * 我们这样 究竟要持续到何时 要走向何方
         * 僕たちは　そうやって どこまで行くのだろう
         * 
         */
        static UIManager()
        {
            if (!System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser)
                Enabled = false;
            else
                Enabled = true;
        }

        public static bool Enabled;
    }
}
