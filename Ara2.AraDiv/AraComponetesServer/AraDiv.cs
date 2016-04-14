// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;
using System.ComponentModel;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vBase:true)]
    public class AraDiv : AraComponentVisualAnchorConteiner, IAraDev
    {

        public AraDiv(IAraObject ConteinerFather)
            : this(AraObjectClienteServer.Create(ConteinerFather, "Div"), ConteinerFather)
        {
            this._MinWidth = 10;
            this._MinHeight = 10;
            this._Width = 100;
            this._Height = 100;
        }

        public AraDiv(string vNameObject, IAraObject vConteinerFather)
            : base(vNameObject, vConteinerFather, "AraDiv")
        {
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            IsVisible = new AraComponentEvent<EventHandler>(this, "IsVisible");
            this.EventInternal += AraDiv_EventInternal;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraDiv/AraDiv.js");
        }

        public void AraDiv_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
                case "ISVISIBLE":
                    IsVisible.InvokeEvent(this, new EventArgs());
                    break;
            }
        }

        public object Tag;

        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click;

        [AraDevEvent]
        public AraComponentEvent<EventHandler> IsVisible;

        event EventHandler _ClickLink;
        public EventHandler ClickLink
        {
            set { _ClickLink = value; }
            get { return _ClickLink; }
        }

        #endregion


        public void RemoveInterface()
        {
            TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.RemoveInterface(); \n");
        }

        private string _Text = "";

        [AraDevProperty("")]
        [PropertySupportLayout]
        public string Text
        {
            set
            {
                if (this.Childs.Where(a => !(a is AraDraggable || a is AraResizable || a is AraAnchor)).Count() > 0)
                    return;

                _Text = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetValue('" + AraTools.StringToStringJS(_Text) + "'); \n");
            }
            get { return _Text; }
        }

        private bool _StyleContainer = false;

        [AraDevProperty(false)]
        [PropertySupportLayout]
        public bool StyleContainer
        {
            set
            {
                _StyleContainer = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetStyleContainer(" + (_StyleContainer ? "true" : "false") + "); \n");
            }
            get { return _StyleContainer; }
        }

        public void CreateLink(string vValue)
        {
            CreateLink(vValue, "");
        }

        public string CreateLink(string vValue, string vCod)
        {
            if (vCod == "") vCod = vValue;
            return @"<a href='#' Click=""javascript:Ara.GetObject('" + InstanceID + "').ClickLink('" + vCod + "');return false;\" >" + vValue + "</a>";
        }

        public void TextAdd(String vNewValue)
        {
            Tick vTick = Tick.GetTick();
            this.TickScriptCall();
            vTick.Script.Send(" vObj.TextAdd('" + AraTools.StringToStringJS(vNewValue) + "'); \n");
        }

        public void TextAddEnd()
        {
            Tick vTick = Tick.GetTick();
            this.TickScriptCall();
            vTick.Script.Send(" vObj.TextAddEnd(); \n");
        }
        
        #region Ara2Dev
        private string _Name = "";

        [AraDevProperty]
        [MergableProperty(false)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys =null;
        public AraEvent<DStartEditPropertys> StartEditPropertys 
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }

        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty 
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }
        #endregion
    }
}
