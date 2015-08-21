using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Util
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DDL runat=server></{0}:DDL>")]
    [Bindable(true)]

    [DefaultValue("")]

    [Themeable(true)]
    [Localizable(true)]
    public class DDL : CompositeControl
    {
        private DropDownList ddlSearch;
        private TextBox txtSearch;
        private DataTable _DataSource;
        string _text = "";
        int _MaxLength = 0;
        public DDL()
        {
            ViewState["width"] = "100%";
            ViewState["Style"] = "";
        }
        
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            EnsureChildControls();

            ddlSearch = new DropDownList();
            //ddlSearch.AutoPostBack = false;
            //ddlSearch.Style[HtmlTextWriterStyle.Width] = Width;
            //ddlSearch.Style[HtmlTextWriterStyle.MarginTop] = "1";
            //string strtest = "Z-INDEX: 1; CLIP: rect(auto auto auto " + (0 - 18) + "px); POSITION: absolute;";
            ////string strtest = "Z-INDEX: 1; POSITION: absolute;";
            //////string strtest = "Z-INDEX: 1; CLIP: rect(auto auto auto 125px); POSITION: absolute;";
            ddlSearch.Style["Style"] = "Z-INDEX: 1;POSITION: absolute;" + Style;
            //ddlSearch.Style["Style"] = "Z-INDEX: 1; CLIP: rect(auto auto auto " + 100 + "px); POSITION: absolute;" + Style;
            //add item
            if (DataSource != null)
            {
                //    ViewState["Sdt"] = DataSource;
                if (_text.Trim() != "")
                {
                    DataRow[] drs = DataSource.Select(DataSource.Columns[0].ColumnName + "='" + _text + "'");
                    if(drs.Length==0)
                    {
                        DataRow dr = DataSource.NewRow();
                        dr[0] = _text;
                        DataSource.Rows.Add(dr);
                    }
                }

                ddlSearch.DataSource = DataSource;
                ddlSearch.DataTextField = DataSource.Columns[0].ColumnName;
                ddlSearch.DataValueField = DataSource.Columns[0].ColumnName;
                ddlSearch.DataBind();
                ddlSearch.SelectedValue = _text;
            }
            ddlSearch.Style[HtmlTextWriterStyle.FontSize] = "9pt";
            ddlSearch.CssClass = "TextBox";


            this.Controls.Add(ddlSearch);

            txtSearch = new TextBox();
            txtSearch.Text = _text;
            txtSearch.MaxLength = _MaxLength;
            txtSearch.Style[HtmlTextWriterStyle.Width] = Width;
            txtSearch.Style["Style"] = Style;
            txtSearch.CssClass = "TextBox";
            this.Controls.Add(txtSearch);
            txtSearch.Attributes.Add("onblur", "JsDDL_changetitle(" + ddlSearch.ClientID + ".id,this.id);");
            ddlSearch.Attributes.Add("Onchange", "document.all." + txtSearch.ClientID + ".parentNode.value = document.all." + ddlSearch.ClientID + ".value;document.all." + txtSearch.ClientID + ".value = document.all." + ddlSearch.ClientID + ".value;ChangePhoneAdress(this,document.all." + txtSearch.ClientID + ");");
            txtSearch.Attributes.Add("onresize", "document.all." + ddlSearch.ClientID + ".style.clip = \"rect(2 auto 16 \" + (document.all." + txtSearch.ClientID + ".offsetWidth -18)+\"px)\";");

            base.CreateChildControls();

        }

        protected override void Render(HtmlTextWriter writer)
        {
            //this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, "1"); 
            base.Render(writer);
            StringBuilder builder = new StringBuilder("");
            //builder.Append("<script type=\"text/javascript\" language=\"javascript\" >");
            //var agent = navigator.userAgent.toLowerCase();
            //if (navigator.userAgent.toLowerCase().indexOf("chrome") > 0) 
            builder.Append("$(document).ready(function (){");
            builder.Append("document.all." + txtSearch.ClientID + ".parentNode.value = document.all." + ddlSearch.ClientID + ".value;document.all." + txtSearch.ClientID + ".value = document.all." + ddlSearch.ClientID + ".value;");
            builder.Append("document.all." + ddlSearch.ClientID + ".style.width = document.all." + txtSearch.ClientID + ".offsetWidth;");
            builder.Append("document.all." + ddlSearch.ClientID + ".style.clip = \"rect(2 auto 16 \" + (document.all." + txtSearch.ClientID + ".offsetWidth -18)+\"px)\";");
            builder.Append(" if (navigator.userAgent.toLowerCase().indexOf(\"chrome\") > 0) {");
            builder.Append("document.all." + ddlSearch.ClientID + ".style.top = document.all." + ddlSearch.ClientID + ".offsetTop - 5;");
            //builder.Append(" } else { ");
            //builder.Append("document.all." + ddlSearch.ClientID + ".style.top = document.all." + txtSearch.ClientID + ".offsetTop;");
            builder.Append("}});");
            //builder.Append("</script>");
            //writer.Write(builder.ToString());
            //this.Page.ClientScript.RegisterForEventValidation(
            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "init" + this.ClientID, builder.ToString(), true);
        }
        protected override void DataBindChildren()
        {
            base.DataBindChildren();
        }

        #region 屬性
        public DataTable DataSource
        {
            set
            {
                this._DataSource = value;
                if (this._DataSource != null && ddlSearch != null)
                {
                    //    ViewState["Sdt"] = DataSource;
                    ddlSearch.DataSource = this._DataSource;
                    ddlSearch.DataTextField = this._DataSource.Columns[0].ColumnName;
                    ddlSearch.DataValueField = this._DataSource.Columns[0].ColumnName;
                    ddlSearch.DataBind();

                    ddlSearch.Style["Style"] = "Z-INDEX: 1;POSITION: absolute;" + Style;
                   

                    // ddlSearch.SelectedValue = Text;
                }
            }
            get { return this._DataSource; }
        }
        public string StrDateFormat
        {
            set
            {
                ViewState["DateFormat"] = value;
            }
        }
        [Category("Misc")]
        public string Width
        {
            set
            {
                ViewState["width"] = value;
            }
            get
            {
                return "" + ViewState["width"];
            }
        }
        [Category("Misc")]
        public string Style
        {
            set
            {
                ViewState["Style"] = value;
            }
            get
            {
                return "" + ViewState["Style"];
            }
        }

        public string Text
        {
            get
            {
                if (txtSearch == null) return _text;
                return txtSearch.Text.Trim();
            }
            set
            {
                _text = value;
                if (txtSearch != null) txtSearch.Text = value;
            }
        }

        public int MaxLength
        {
            get
            {
                if (txtSearch == null) return _MaxLength;
                return txtSearch.MaxLength;
            }
            set
            {
                _MaxLength = value;
            }
        }
        #endregion
    }
}
