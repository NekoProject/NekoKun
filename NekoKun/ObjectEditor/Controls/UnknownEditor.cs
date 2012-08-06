﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class UnknownEditor : UI.LynnLabel, IObjectEditor
    {
        private Object obj;
        public UnknownEditor(Dictionary<string, object> Params)
        {
            if (RequestCommit != null) RequestCommit.ToString();
            if (DirtyChanged != null) DirtyChanged.ToString();
        }

        public void Commit()
        {

        }

        public object SelectedItem
        {
            get
            {
                return this.obj;
            }
            set
            {
                this.obj = value;
                this.Text = (value ?? "").ToString();
            }
        }

        public event EventHandler RequestCommit;

        #region IObjectEditor 成员


        public event EventHandler DirtyChanged;

        #endregion
    }
}
