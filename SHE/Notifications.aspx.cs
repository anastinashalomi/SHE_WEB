using SHE.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = System.Web.UI.WebControls.Image;

namespace SHE
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //String username = (string)Session.Contents["LoggedUser"];
                //che auth
                if (!IsPostBack)
                {
                    if (Session.Contents["LoggedUser"] != null)
                    {
                        // Session variable exists
                        // Perform necessary actions
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
        
                PanelOne.Visible = true;
                PanelTwo.Visible = false;
                List<NotificationDTO> patientList = new List<NotificationDTO>
                {
                    new NotificationDTO { AdmittedType = "updated", PatientName = "John Doe", Hospital = "City Hospital" },
                    new NotificationDTO { AdmittedType = "accepted", PatientName = "Jane Smith", Hospital = "General Hospital" },
                    new NotificationDTO { AdmittedType = "reassign", PatientName = "Sam Johnson", Hospital = "Community Clinic" },
                    new NotificationDTO { AdmittedType = "pending", PatientName = "Sam Johnson", Hospital = "Community Clinic" },
                    
                };
                
                NotificationGrid.DataSource = patientList;
                NotificationGrid.DataBind();
            }
        }

        protected void NotificationGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(NotificationGrid, "Select$" + e.Row.RowIndex);
                //event validations
                e.Row.Style["cursor"] = "pointer";
                NotificationDTO patient = (NotificationDTO)e.Row.DataItem;
                e.Row.CssClass = GetAdmittedTypeCssClass(patient.AdmittedType);
                Image image = (Image)e.Row.FindControl("NotificationImage");



                // Set icon image based on AdmittedType
                switch (patient.AdmittedType)
                {
                    case "updated":
                        image.ImageUrl =  "~/images/Login.png";
                        break;
                    case "accepted":
                        image.ImageUrl = "~/images/bee.jpg";
                        break;
                    case "reassign":
                        image.ImageUrl = "~/images/slic.jpg";
                        break;
                    case "pending":
                        image.ImageUrl = "~/images/slic.jpg";
                        break;
                    default:
                        //image.ImageUrl = "~/images/slic.jpg";

                        break;
                }
            }
        }

        private string GetAdmittedTypeCssClass(string admittedType)
        {
            switch (admittedType)
            {
                case "updated":
                    return "alert alert-success";
                case "accepted":
                    return "alert alert-dark";
                case "reassign":
                    return "alert alert-danger";
                case "pending":
                    return "alert alert-primary";
                default:
                    return string.Empty;
            }
        }

 

        protected void NotificationGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void NotificationGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NotificationGrid.SelectedIndex >= 0)
            {
               
                // Get the selected row index and retrieve the data from the GridView
                int rowIndex = NotificationGrid.SelectedIndex;
                GridViewRow selectedRow = NotificationGrid.Rows[rowIndex];

                // Retrieve data from the row and use it as needed
                string data = ((Label)selectedRow.FindControl("AdmitType")).Text; 


              
                PanelOne.Visible = false;
                PanelTwo.Visible = true;
             
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            PanelOne.Visible = true;
            PanelTwo.Visible = false;
        }
    }
}