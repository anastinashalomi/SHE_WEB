﻿using SHE.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHE.ClaimPayment
{
    public partial class ClaimSearch : System.Web.UI.Page
    {

        EncryptDecrypt dc = new EncryptDecrypt();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["policy"]) && !string.IsNullOrEmpty(Request.QueryString["epf"]) && !string.IsNullOrEmpty(Request.QueryString["claimRef"]))
                {
                    string policy = Request.QueryString["policy"];
                    string epfno = Request.QueryString["epf"];
                    string claimRefNo = Request.QueryString["claimRef"];
                    

                    policy = dc.Decrypt(policy);
                    epfno = dc.Decrypt(epfno);
                    claimRefNo = dc.Decrypt(claimRefNo);

                    policyno.Value = policy;
                    epf.Value = epfno;
                    claimRef.Value = claimRefNo;

                    claimRef.Disabled = true;
                    epf.Disabled = true;
                    policyno.Disabled = true;
                }
                else
                {
                    //epflbl.Visible = false;
                    //epf.Visible = false;
                    claimRef.Visible = false;
                    lblClaimRef.Visible = false;
                }
                

                if (!string.IsNullOrEmpty(Request.QueryString["alert"]))
                {
                    lblAlertMessage.Text = Request.QueryString["alert"];
                    lblAlertMessage.CssClass = "alert alert-warning"; // Add CSS class for styling
                    lblAlertMessage.Attributes.Add("data-alert-type", "custom"); // Add custom attribute to identify the alert type
                    lblAlertMessage.Visible = true;

                    
                }
            }

        }

        protected void claimPayment_submit_Click(object sender, EventArgs e)
        {
            string policy = policyno.Value;
            string epfno = epf.Value;
            string claimRefNo= claimRef.Value;
            string fromNotifi = Request.QueryString["fromNotifi"];

            if ((policy == null || policy == "") && (epfno == null || epfno == "" ) && (claimRefNo == "" || claimRefNo == null))
            {
                //error2.Visible = true;
            }
            else if (claimRefNo == null || claimRefNo == "")
            {
                Response.Redirect("~/ClaimPayment/ClaimPayList.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno) );
            }
            else
            {
                policyno.Disabled = true;
                epf.Disabled = true;
                claimRef.Disabled = true;
                
                Response.Redirect("~/ClaimPayment/ClaimPaymentDetail.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno) +"&CLAIMREF=" + dc.Encrypt(claimRefNo) + "&fromNotifi=" + fromNotifi);
            }
            
        }

        protected void back_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["policy"]) && !string.IsNullOrEmpty(Request.QueryString["epf"]) && !string.IsNullOrEmpty(Request.QueryString["claimRef"]))
            {
                Response.Redirect("~/Notifications.aspx");
            }
            else
            {
                Response.Redirect("~/default.aspx");
            }
               
        }

        



    }
}