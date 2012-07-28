using System;
using System.Collections.Generic;
using System.Text;
using ScintillaNet;
using System.Drawing;

namespace NekoKun.UI
{
	public class RubyScintilla : Scintilla
	{
		public RubyScintilla()
		{
			// http://ondineyuga.com/svn/RGE2/Tools/RGESEditor/RGESEditor_lang/EditorScintilla/Scintilla.cs
			// line number
			this.Margins[0].Width = 39;

			// fold
			this.Margins[1].Type = MarginType.Symbol;
			this.Margins[1].Mask = -33554432; //SC_MASK_FOLDERS
			this.Margins[1].Width = 16;
			this.Margins[1].IsClickable = true;
			this.NativeInterface.SetProperty("fold", "1");
			this.NativeInterface.SetProperty("fold.comment", "0");
			this.NativeInterface.SetProperty("fold.compact", "1");
			this.NativeInterface.SetProperty("fold.preprocessor", "1");

			this.Folding.Flags = FoldFlag.LineAfterContracted;

			// lexing
			this.Lexing.Lexer = Lexer.Ruby;
			this.Lexing.SetKeywords(0, "__FILE__ __LINE__ BEGIN END alias and begin break case class def defined? do else elsif end ensure false for if in module next nil not or redo rescue retry return self super then true undef unless until when while yield");
			this.UseFont = true;
			this.Styles[(int)SCE_RB.DEFAULT].ForeColor = Color.FromArgb(0, 0, 0);
			this.Styles[(int)SCE_RB.DEFAULT].BackColor = Color.FromArgb(255, 255, 255);
			this.Styles[(int)SCE_RB.WORD].ForeColor = Color.FromArgb(0, 0, 127);
			this.Styles[(int)SCE_RB.WORD_DEMOTED].ForeColor = Color.FromArgb(0, 0, 127);
			this.Styles[(int)SCE_RB.STRING].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.GLOBAL].ForeColor = Color.FromArgb(180, 0, 180);
			this.Styles[(int)SCE_RB.CLASSNAME].ForeColor = Color.FromArgb(0, 0, 255);
			this.Styles[(int)SCE_RB.MODULE_NAME].ForeColor = Color.FromArgb(160, 0, 160);
			this.Styles[(int)SCE_RB.CLASS_VAR].ForeColor = Color.FromArgb(128, 0, 204);
			this.Styles[(int)SCE_RB.INSTANCE_VAR].ForeColor = Color.FromArgb(176, 0, 128);
			this.Styles[(int)SCE_RB.NUMBER].ForeColor = Color.FromArgb(0, 127, 127);
			this.Styles[(int)SCE_RB.STRING_Q].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.STRING_QQ].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.STRING_QX].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.STRING_QR].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.STRING_QW].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.REGEX].ForeColor = Color.FromArgb(120, 0, 170);
			this.Styles[(int)SCE_RB.SYMBOL].ForeColor = Color.FromArgb(205, 100, 30);
			this.Styles[(int)SCE_RB.DEFNAME].ForeColor = Color.FromArgb(0, 127, 127);
			this.Styles[(int)SCE_RB.BACKTICKS].ForeColor = Color.FromArgb(160, 65, 10);
			this.Styles[(int)SCE_RB.HERE_DELIM].ForeColor = Color.FromArgb(0, 137, 0);
			this.Styles[(int)SCE_RB.HERE_Q].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.HERE_QQ].ForeColor = Color.FromArgb(127, 0, 151);
			this.Styles[(int)SCE_RB.HERE_QX].ForeColor = Color.FromArgb(0, 137, 0);
			this.Styles[(int)SCE_RB.DATASECTION].ForeColor = Color.FromArgb(127, 0, 0);
			this.Styles[(int)SCE_RB.COMMENTLINE].ForeColor = Color.FromArgb(0, 127, 0);

			this.EndOfLine.Mode = EndOfLineMode.Crlf;
			this.LineWrap.Mode = WrapMode.None;

			this.Indentation.UseTabs = false;
			this.Indentation.TabIndents = true;
			this.Indentation.TabWidth = 2;
			this.Indentation.ShowGuides = false;
			this.Indentation.BackspaceUnindents = true;
			this.Indentation.IndentWidth = 2;
			this.Indentation.SmartIndentType = SmartIndent.Simple;

			this.LongLines.EdgeMode = EdgeMode.Line;
			this.LongLines.EdgeColumn = 160;

			this.Caret.HighlightCurrentLine = true;
			this.Caret.CurrentLineBackgroundColor = Color.FromArgb(240, 240, 240);
		}

		public enum SCE_RB
		{
			DEFAULT = 0,
			ERROR = 1,
			COMMENTLINE = 2,
			POD = 3,
			NUMBER = 4,
			WORD = 5,
			STRING = 6,
			CHARACTER = 7,
			CLASSNAME = 8,
			DEFNAME = 9,
			OPERATOR = 10,
			IDENTIFIER = 11,
			REGEX = 12,
			GLOBAL = 13,
			SYMBOL = 14,
			MODULE_NAME = 15,
			INSTANCE_VAR = 16,
			CLASS_VAR = 17,
			BACKTICKS = 18,
			DATASECTION = 19,
			HERE_DELIM = 20,
			HERE_Q = 21,
			HERE_QQ = 22,
			HERE_QX = 23,
			STRING_Q = 24,
			STRING_QQ = 25,
			STRING_QX = 26,
			STRING_QR = 27,
			STRING_QW = 28,
			WORD_DEMOTED = 29,
			STDIN = 30,
			STDOUT = 31,
			STDERR = 40,
			UPPER_BOUND = 41
		}
	}

}
