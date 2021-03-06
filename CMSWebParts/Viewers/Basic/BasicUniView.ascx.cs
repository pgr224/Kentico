using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalControls;
using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.Controls;
using CMS.ExtendedControls;

public partial class CMSWebParts_Viewers_Basic_BasicUniView : CMSAbstractWebPart
{
    #region "Variables"

    // Base datasource instance
    private CMSBaseDataSource mDataSourceControl = null;
    // Indicates whether control was binded
    private bool binded = false;
    // BasicRepeter instance
    BasicUniView basicUniView = new BasicUniView();
    // Indicates whether current control was added to the filter collection
    private bool mFilterControlAdded = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets name of source.
    /// </summary>
    public string DataSourceName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("DataSourceName"), String.Empty);
        }
        set
        {
            this.SetValue("DataSourceName", value);
        }
    }


    /// <summary>
    /// Control with data source.
    /// </summary>
    public CMSBaseDataSource DataSourceControl
    {
        get
        {
            // Check if control is empty and load it with the data
            if (this.mDataSourceControl == null)
            {
                if (!String.IsNullOrEmpty(this.DataSourceName))
                {
                    this.mDataSourceControl = CMSControlsHelper.GetFilter(this.DataSourceName) as CMSBaseDataSource;
                }
            }

            return this.mDataSourceControl;
        }
        set
        {
            this.mDataSourceControl = value;
        }
    }


    /// <summary>
    /// Gets or sets FirstItemTransformationName property.
    /// </summary>
    public string FirstItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("FirstItemTransformationName"), "");
        }
        set
        {
            this.SetValue("FirstItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets LastItemTransformationName property.
    /// </summary>
    public string LastItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("LastItemTransformationName"), "");
        }
        set
        {
            this.SetValue("LastItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets AlternatingItemTemplate property.
    /// </summary>
    public string AlternatingItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("AlternatingItemTransformationName"), "");
        }
        set
        {
            this.SetValue("AlternatingItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate property.
    /// </summary>
    public string FooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("FooterTransformationName"), "");
        }
        set
        {
            this.SetValue("FooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate property.
    /// </summary>
    public string HeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("HeaderTransformationName"), "");
        }
        set
        {
            this.SetValue("HeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate property.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("TransformationName"), "");
        }
        set
        {
            this.SetValue("TransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets SeparatorTemplate property.
    /// </summary>
    public string SeparatorTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SeparatorTransformationName"), "");
        }
        set
        {
            this.SetValue("SeparatorTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HideControlForZeroRows property.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(this.GetValue("HideControlForZeroRows"), true);
        }
        set
        {
            this.SetValue("HideControlForZeroRows", value);
        }
    }


    /// <summary>
    /// Gets or sets ZeroRowsText property.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("ZeroRowsText"), "");
        }
        set
        {
            this.SetValue("ZeroRowsText", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate for selected item.
    /// </summary>
    public string SelectedItemFooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SelectedItemFooterTransformationName"), "");
        }
        set
        {
            this.SetValue("SelectedItemFooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate for selected item.
    /// </summary>
    public string SelectedItemHeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SelectedItemHeaderTransformationName"), "");
        }
        set
        {
            this.SetValue("SelectedItemHeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate for selected item.
    /// </summary>
    public string SelectedItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SelectedItemTransformationName"), "");
        }
        set
        {
            this.SetValue("SelectedItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate for single item.
    /// </summary>
    public string SingleItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SingleItemTransformationName"), String.Empty);
        }
        set
        {
            this.SetValue("SingleItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether header and footer items should be hidden if single item is displayed.
    /// </summary>
    public bool HideHeaderAndFooterForSingleItem
    {
        get
        {
            return ValidationHelper.GetBoolean(this.GetValue("HideHeaderAndFooterForSingleItem"), this.basicUniView.HideHeaderAndFooterForSingleItem);
        }
        set
        {
            this.SetValue("HideHeaderAndFooterForSingleItem", value);
            this.basicUniView.HideHeaderAndFooterForSingleItem = value;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// On content loaded override.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Set properties
            if (!String.IsNullOrEmpty(this.ZeroRowsText))
            {
                this.basicUniView.ZeroRowsText = this.ZeroRowsText;
            }

            this.basicUniView.HideControlForZeroRows = this.HideControlForZeroRows;
            this.basicUniView.HideHeaderAndFooterForSingleItem = this.HideHeaderAndFooterForSingleItem;
            this.basicUniView.DataBindByDefault = false;
            this.basicUniView.OnPageChanged += new EventHandler<EventArgs>(BasicRepeater_OnPageChanged);

            EnsureFilterControl();
        }
    }


    /// <summary>
    /// Ensures current control in the filters collection.
    /// </summary>
    protected void EnsureFilterControl()
    {
        if (!mFilterControlAdded)
        {
            // Add basic repeater to the filter collection
            CMSControlsHelper.SetFilter(ValidationHelper.GetString(this.GetValue("WebPartControlID"), this.ClientID), this.basicUniView);
            mFilterControlAdded = true;
        }
    }


    /// <summary>
    /// OnPageChaged event handler.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArg</param>
    void BasicRepeater_OnPageChanged(object sender, EventArgs e)
    {
        // Reload data
        if (this.DataSourceControl != null)
        {
            this.basicUniView.DataSource = this.DataSourceControl.DataSource;
            LoadTransformations();
            this.basicUniView.DataBind();
            binded = true;
        }
    }


    /// <summary>
    /// Loads and setups web part.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        // Add control to the control collection
        plcBasicUniView.Controls.Add(this.basicUniView);
        
        // Check whether postback was executed from current transformation item
        if (RequestHelper.IsPostBack())
        {
            // Indicates whether postback was fired from current control
            bool bindControl = false;

            // Check event target value and callback parameter value
            string eventTarget = ValidationHelper.GetString(this.Request.Form["__EVENTTARGET"], String.Empty);
            string callbackParam = ValidationHelper.GetString(this.Request.Form["__CALLBACKPARAM"], String.Empty);
            if (eventTarget.StartsWith(this.UniqueID) || callbackParam.StartsWith(this.UniqueID) || eventTarget.EndsWith(ContextMenu.CONTEXT_MENU_SUFFIX))
            {
                bindControl = true;
            }
            // Check whether request key contains some control assigned to current control
            else
            {
                foreach (string key in this.Request.Form.Keys)
                {
                    if ((key != null) && key.StartsWith(this.UniqueID))
                    {
                        bindControl = true;
                        break;
                    }
                }
            }

            if (bindControl)
            {
                // Reload data
                if (this.DataSourceControl != null)
                {
                    this.basicUniView.DataSource = this.DataSourceControl.DataSource;
                    this.LoadTransformations();
                    this.basicUniView.DataBind();
                    binded = true;
                }
            }
        }

        // Handle filter change event
        if (this.DataSourceControl != null)
        {
            this.DataSourceControl.OnFilterChanged += new ActionEventHandler(DataSourceControl_OnFilterChanged);
        }

        base.OnLoad(e);
    }


    /// <summary>
    /// Load transformations with dependence on datasource type and datasource state.
    /// </summary>
    protected void LoadTransformations()
    {
        CMSBaseDataSource docDataSource = this.DataSourceControl as CMSBaseDataSource;
        if ((docDataSource != null) && (docDataSource.IsSelected) && (!String.IsNullOrEmpty(this.SelectedItemTransformationName)))
        {
            this.basicUniView.ItemTemplate = CMSDataProperties.LoadTransformation(this, this.SelectedItemTransformationName, false);

            if (!String.IsNullOrEmpty(this.SelectedItemFooterTransformationName))
            {
                this.basicUniView.FooterTemplate = CMSDataProperties.LoadTransformation(this, this.SelectedItemFooterTransformationName, false);
            }
            else
            {
                this.basicUniView.FooterTemplate = null;
            }

            if (!String.IsNullOrEmpty(this.SelectedItemHeaderTransformationName))
            {
                this.basicUniView.HeaderTemplate = CMSDataProperties.LoadTransformation(this, this.SelectedItemHeaderTransformationName, false);
            }
            else
            {
                this.basicUniView.HeaderTemplate = null;
            }
        }
        else
        {
            // Apply transformations if they exist
            if (!String.IsNullOrEmpty(this.TransformationName))
            {
                this.basicUniView.ItemTemplate = CMSDataProperties.LoadTransformation(this, this.TransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.AlternatingItemTransformationName))
            {
                this.basicUniView.AlternatingItemTemplate = CMSDataProperties.LoadTransformation(this, this.AlternatingItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.FooterTransformationName))
            {
                this.basicUniView.FooterTemplate = CMSDataProperties.LoadTransformation(this, this.FooterTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.HeaderTransformationName))
            {
                this.basicUniView.HeaderTemplate = CMSDataProperties.LoadTransformation(this, this.HeaderTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.SeparatorTransformationName))
            {
                this.basicUniView.SeparatorTemplate = CMSDataProperties.LoadTransformation(this, this.SeparatorTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.FirstItemTransformationName))
            {
                this.basicUniView.FirstItemTemplate = CMSDataProperties.LoadTransformation(this, this.FirstItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.LastItemTransformationName))
            {
                this.basicUniView.LastItemTemplate = CMSDataProperties.LoadTransformation(this, this.LastItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(this.SingleItemTransformationName))
            {
                this.basicUniView.SingleItemTemplate = CMSDataProperties.LoadTransformation(this, this.SingleItemTransformationName, false);
            }
        }
    }


    /// <summary>
    /// OnFilter change event handler.
    /// </summary>
    void DataSourceControl_OnFilterChanged()
    {
        // Set forcibly visibility
        this.Visible = true;

        // Reload data
        if (this.DataSourceControl != null)
        {
            this.basicUniView.DataSource = this.DataSourceControl.DataSource;
            LoadTransformations();
            this.basicUniView.DataBind();
            binded = true;
        }
    }


    /// <summary>
    /// OnPreRender override.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        // Datasource data
        object ds = null;

        // Set transformations if data source is not empty
        if (this.DataSourceControl != null)
        {
            // Get data from datasource
            ds = this.DataSourceControl.DataSource;

            // Check whether data exist
            if ((!DataHelper.DataSourceIsEmpty(ds)) && (!binded))
            {
                // Initilaize related data if provided
                if (this.DataSourceControl.RelatedData != null)
                {
                    this.RelatedData = this.DataSourceControl.RelatedData;
                }

                this.basicUniView.DataSource = this.DataSourceControl.DataSource;
                this.LoadTransformations();
                this.basicUniView.DataBind();
            }
        }

        base.OnPreRender(e);

        // Hide control for zero rows
        if (((this.DataSourceControl == null) || (DataHelper.DataSourceIsEmpty(ds))) && (this.HideControlForZeroRows))
        {
            this.Visible = false;
        }
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
        EnsureFilterControl();
        base.ReloadData();
    }

    #endregion;
}
