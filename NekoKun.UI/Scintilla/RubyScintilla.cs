﻿using System;
using System.Collections.Generic;
using System.Text;
using ScintillaNET;
using System.Drawing;

namespace NekoKun.UI
{
	public class RubyScintilla : Scintilla
    {
        private static List<char> braces = new List<char> { '(', ')', '[', ']', '{', '}' };
        private static List<char> suppressedChars = new List<char> { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '}', '[', ']', ':', ';', '<', '>', '.', ',', '?', '/', '\\', '\n', '\r', '\t', '`', '~', '|', '\'', '"' };
        private static List<string> unindentWords = new List<string> { "else", "elsif", "rescue", "ensure", "when", "end", ")", "]", "}" };

		public RubyScintilla()
		{

            //this.ConfigurationManager.Language = "ruby";

			// http://ondineyuga.com/svn/RGE2/Tools/RGESEditor/RGESEditor_lang/EditorScintilla/Scintilla.cs

			// fold
			this.Margins[1].Mask = -33554432; //SC_MASK_FOLDERS
            this.Margins[1].Width = 16;
            this.Margins[1].IsClickable = true;

            
			// lexing
			this.Lexing.Lexer = Lexer.Ruby;
			this.Lexing.SetKeywords(0, "__FILE__ __LINE__ BEGIN END alias and begin break case class def defined? do else elsif end ensure false for if in module next nil not or redo rescue retry return self super then true undef unless until when while yield");
            this.Lexing.LineCommentPrefix = "#~ ";

            this.Folding.MarkerScheme = FoldMarkerScheme.Arrow;
            this.Folding.UseCompactFolding = true;
            this.Folding.Flags = FoldFlag.LineAfterContracted;
            this.Folding.IsEnabled = true;

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
            this.LineWrapping.Mode = LineWrappingMode.None;

			this.Indentation.UseTabs = false;
			this.Indentation.TabIndents = true;
			this.Indentation.TabWidth = 2;
            this.Indentation.ShowGuides = true;
			this.Indentation.BackspaceUnindents = true;
			this.Indentation.IndentWidth = 2;

			this.LongLines.EdgeMode = EdgeMode.Line;
			this.LongLines.EdgeColumn = 160;

			this.Caret.HighlightCurrentLine = true;
			this.Caret.CurrentLineBackgroundColor = Color.FromArgb(240, 240, 240);

            this.CharAdded += new EventHandler<CharAddedEventArgs>(RubyScintilla_CharAdded);

            this.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripMenuItem(
                "智能格式化(&S)", null,
                delegate
                {
                    this.SmartIndent();
                }
            ));
            this.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripMenuItem(
                "切换注释(&Q)", null,
                delegate
                {
                    this.ToggleComment();
                }, 
                System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q
            ));
		}

        private void ToggleComment()
        {
            this.Commands.Execute(BindableCommand.ToggleLineComment);
        }

        void RubyScintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            if (e.Ch == '\n')
            {
                string lastline = this.Lines.Current.Previous.Text.Trim();
                if (lastline == "=begin" || lastline == "=end")
                    this.Lines.Current.Previous.Indentation = 0;
                else
                {
                    int num = this.GetLineIndent(Lines.Current.Previous);
                    if (num != -1) { Lines.Current.Previous.Indentation = num * Indentation.TabWidth; }
                }
                int lineIndent = this.GetLineIndent(Lines.Current);
                if (lineIndent != -1)
                    this.InsertText(new string(' ', lineIndent * Indentation.TabWidth));
            }
        }

        private int GetLineIndent(Line line)
        {
            int num = line.StartPosition - 1;
            this.NativeInterface.Colourise(num, num + 1);
            int styleAt = (int)this.Styles.GetStyleAt(num);
            if (styleAt == 3 || styleAt == 6 || styleAt == 7 || styleAt == 12 || styleAt == 18 || line.Text.StartsWith("=begin"))
            {
                return -1;
            }
            int indent = line.FoldLevel - 1024;
            string word = this.GetWordFromPosition(line.IndentPosition);
            string item = this.CharAt(line.IndentPosition).ToString();
            if (unindentWords.Contains(word) || unindentWords.Contains(item))
            {
                indent--;
            }
            return indent;
        }

        public void SmartIndent()
        {
            this.UndoRedo.BeginUndoAction();
            foreach (Line line in this.Lines)
            {
                int lineIndent = this.GetLineIndent(line);
                if (lineIndent != -1)
                {
                    line.Indentation = 0;
                    line.Indentation = lineIndent * this.Indentation.TabWidth;
                }
            }
            this.UndoRedo.EndUndoAction();
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
