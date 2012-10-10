using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyRegexp
    {
        public RubyRegexpOptions Options;
        public RubyString Pattern;

        public RubyRegexp(RubyString Pattern, RubyRegexpOptions Options)
        {
            this.Pattern = Pattern;
            this.Options = Options;
        }
    }

    [Flags]
    public enum RubyRegexpOptions
    {
        None = 0,   // #define ONIG_OPTION_NONE               0U
        IgnoreCase = 1,   // #define ONIG_OPTION_IGNORECASE         1U
        Extend = 2,   // #define ONIG_OPTION_EXTEND             (ONIG_OPTION_IGNORECASE         << 1)
        Multiline = 4,   // #define ONIG_OPTION_MULTILINE          (ONIG_OPTION_EXTEND             << 1)
        Singleline = 8,   // #define ONIG_OPTION_SINGLELINE         (ONIG_OPTION_MULTILINE          << 1)
        FindLongest = 16,  // #define ONIG_OPTION_FIND_LONGEST       (ONIG_OPTION_SINGLELINE         << 1)
        FindNotEmpty = 32,  // #define ONIG_OPTION_FIND_NOT_EMPTY     (ONIG_OPTION_FIND_LONGEST       << 1)
    }
}
