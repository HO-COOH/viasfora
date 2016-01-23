﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Utilities;
using Winterdom.Viasfora.Rainbow;
using Winterdom.Viasfora.Util;

namespace Winterdom.Viasfora.Contracts {
  public interface ILanguage {
    String[] ControlFlow { get; set; }
    String[] Linq { get; set; }
    String[] Visibility { get; set; }
    String KeyName { get; }
    bool Enabled { get; set; }

    IBraceScanner NewBraceScanner();
    IFirstLineCommentParser NewFirstLineCommentParser();
    IStringScanner NewStringScanner(String text);
    bool IsControlFlowKeyword(String text);
    bool IsVisibilityKeyword(String text);
    bool IsLinqKeyword(String text);
    bool MatchesContentType(IContentType contentType);
  }
}
