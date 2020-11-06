using FastContact.fastcontactClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastContact
{
    public partial class FastContact : Form
    {
        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);

        public FastContact()
        {
            InitializeComponent();
        }

        contactClass c = new contactClass();
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the value from the input fields

            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtboxContactNumber.Text;
            c.Address = txtboxAddress.Text;
            c.Gender = cmbGender.Text;

            //Inserting Data into Database using the mehtod we created previously

            bool success = c.Insert(c);
            if(success==true)
            {
                //Successfully inserted
                MessageBox.Show("New Contact Succesfully Inserted");
                //Call the Method here
                Clear();
            }

            else
            {
                //Failed to add contact
                MessageBox.Show("Failed to add New Contact. Try Again.");
            }

            //Load Data on Data Grid view

            DataTable dt = c.Select();
            dgvContactList.DataSource = dt; 
        }

        private void FastContact_Load(object sender, EventArgs e)
        {
            //Load Data on Data Grid view

            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Method to CLEAR fields
        public void Clear()
        {
            txtboxFirstName.Text = "";
            txtboxLastName.Text = "";
            txtboxContactNumber.Text = "";
            txtboxAddress.Text = "";
            cmbGender.Text = "";
            txtboxContactID.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the Data from textboxes
            c.ContactID = int.Parse(txtboxContactID.Text);
            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtboxContactNumber.Text;
            c.Address = txtboxAddress.Text;
            c.Gender = cmbGender.Text;

            //Update Data in Database

            bool succes = c.Update(c);
            if (succes == true)
            {
                //Updated Succesfully
                MessageBox.Show("Contact has been succesfully updated.");

                //Refresh list

                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
            else
            {
                //Failed to Update
                MessageBox.Show("Failed to update contact.Try again.");
            }
        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the Data from Data Grid View and Load it in the textboxes respectively
            //Identify the row on which mouse is clicked

            int rowIndex = e.RowIndex;
            txtboxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            txtboxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            txtboxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            txtboxContactNumber.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            txtboxAddress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();



        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Call Clear Method here
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get Data from the Textboxes

            c.ContactID = Convert.ToInt32(txtboxContactID.Text);
            bool success = c.Delete(c);
            if (success == true)
            {
                //Succesfully Deleted
                MessageBox.Show("Contact succesfully deleted.");
                
                //Refresh list
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;

                //Call method to clear textboxes
                Clear();
            }
            else
            {
                //Failed to delete
                MessageBox.Show("Failed to delete contact. Try Again.");
            }
        }
        static string myconnstr = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the value from text box
            string keyword = txtboxSearch.Text;
          
            SqlConnection conn = new SqlConnection(myconnstr);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE FirstName LIKE '%"+keyword+"%' OR LastName LIKE '%" +keyword+ "%' OR Address LIKE '%" + keyword + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        //Make window movable across screen

        private void FastContact_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void FastContact_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void FastContact_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

    }
}
