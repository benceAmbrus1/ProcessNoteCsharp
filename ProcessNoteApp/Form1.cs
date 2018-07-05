using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessNoteApp
{
    public partial class Form1 : Form
    {
        private Process[] processes;
        private PerformanceCounter theCPUCounter;
        private Process p;
        private List<String> messages = new List<String>();
            
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            processes = System.Diagnostics.Process.GetProcesses();
            Array.Sort(processes,
            delegate (System.Diagnostics.Process x, System.Diagnostics.Process y) { return x.ProcessName.CompareTo(y.ProcessName); });

            foreach (System.Diagnostics.Process process in processes)
            {
                listBox1.Items.Add("PID: " + process.Id + ": " + process.ProcessName);

            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Visible = true;
                button1.Click += new EventHandler(delete_clicked);
                treeView1.Nodes.Clear();
                p = processes[listBox1.SelectedIndex];

                theCPUCounter = new PerformanceCounter();
                label1.Text = p.ProcessName;
                theCPUCounter.CategoryName = "Process";
                theCPUCounter.CounterName = "% Processor Time";
                theCPUCounter.InstanceName = p.ProcessName;
                TreeNode treeNode = new TreeNode("CPU usage: " + p.TotalProcessorTime);
                TreeNode treeNode2 = new TreeNode("Memory usage: " + (p.WorkingSet64 / (1024*1024)) + " MB");
                TreeNode treeNode3 = new TreeNode("Running time: " + (DateTime.Now - p.StartTime));
                TreeNode treeNode4 = new TreeNode("Start time: " + p.StartTime);
                List<TreeNode> list = new List<TreeNode>();
                foreach (ProcessThread t in p.Threads)
                {
                    TreeNode tempThreadNode = new TreeNode("Thread ID: " + t.Id);
                    list.Add(tempThreadNode);
                }
                TreeNode treeNode5 = new TreeNode("Threads", list.ToArray());

                treeView1.Nodes.Add(treeNode);
                treeView1.Nodes.Add(treeNode2);
                treeView1.Nodes.Add(treeNode3);
                treeView1.Nodes.Add(treeNode4);
                treeView1.Nodes.Add(treeNode5);

            }
            catch (Exception)
            {
                TreeNode treeNode = new TreeNode("Access Denied");
                treeView1.Nodes.Add(treeNode);
            }
        }

        private void delete_clicked(object sender, EventArgs e)
        {
            processes[listBox1.SelectedIndex].Kill();
            listBox1.Items.Clear();
            processes = System.Diagnostics.Process.GetProcesses();
            Array.Sort(processes, delegate (System.Diagnostics.Process x, System.Diagnostics.Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
            foreach (System.Diagnostics.Process process in processes)
            {
                listBox1.Items.Add("PID: " + process.Id + ": " + process.ProcessName);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            processes = System.Diagnostics.Process.GetProcesses();
            Array.Sort(processes,
            delegate (System.Diagnostics.Process x, System.Diagnostics.Process y) { return x.ProcessName.CompareTo(y.ProcessName); });

            foreach (System.Diagnostics.Process process in processes)
            {
                listBox1.Items.Add("PID: " + process.Id + ": " + process.ProcessName);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(textBox1.Text))
            {
                String message = DateTime.Now + " | " + textBox1.Text;
                messages.Add(message);
                listBox2.Items.Add(message);
                textBox1.Clear();
            }
        }
    }
}
