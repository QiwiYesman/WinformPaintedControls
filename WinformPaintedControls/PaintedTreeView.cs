using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintedControls
{
    public class PaintedTreeView: TreeView
    {
        [Category("Colors"), Description("Back colors"),
          Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer BackColors { get; set; }

        [Category("Colors"), Description("Text and border colors"),
           Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer ForeColors { get; set; }

        public Color PlusRectColor { get; set; }
        public Color MinusRectColor { get; set; }

        void InitColors()
        {
            BackColors = new()
            {
                DefaultColor = Color.Black,
                HoverColor = Color.DarkBlue,
                PressColor = Color.LightGreen
            };

            ForeColors = new()
            {
                DefaultColor = Color.White,
                HoverColor = Color.White,
                PressColor = Color.DarkBlue
            };
            LineColor = Color.Red;
            BackColor = BackColors.DefaultColor;
            PlusRectColor = LineColor;
            MinusRectColor = Color.LightCyan;
            BackColors.ColorChanged += (e) => BackColor = BackColors.DefaultColor;
        }

        void InitFlags()
        {
            DrawMode = TreeViewDrawMode.OwnerDrawAll;
            HotTracking = true;
            BorderStyle = BorderStyle.None;
            FullRowSelect = false;
        }

        public PaintedTreeView()
        {
            InitFlags();
            InitColors();
        }
        (Rectangle, Rectangle) GetFirstAndLastNodeBounds(TreeNode node)
        {
            if(node.Parent ==null)
            {
                return (Nodes[0].Bounds, Nodes[^1].Bounds);
            }
            return (node.Parent.FirstNode.Bounds, node.Parent.LastNode.Bounds);
        }
        TreeNode GetFirstNode(TreeNode nodeLevel)
        {
            if (nodeLevel.Parent == null)
            {
                return Nodes[0];
            }
            return nodeLevel.Parent.FirstNode;
        }
        void RepaintNodeLevel(TreeNode node, Graphics e, Pen linePen)
        {
            (var first, var last) = GetFirstAndLastNodeBounds(node);
            e.DrawLine(linePen,
                new(last.X - Indent / 2, last.Y + last.Height / 2),
                new(first.X - Indent / 2, first.Y + first.Height / 2)
                );
            RepaintNodeLevelPlusMinus(node, e);
        }

        void RepaintNodeLevelPlusMinus(TreeNode node, Graphics e)
        {
            var first = GetFirstNode(node);
            while (first != null)
            {
                RepaintNodePlusMinus(first, e);
                first = first.NextNode;
            }
 
        }

        void RepaintNodePlusMinus(TreeNode node, Graphics e)
        {
            if (node.Nodes.Count != 0)
            {
                var b = node.Bounds;
                using var minusplusBrush = node.IsExpanded == true ?
                    new SolidBrush(MinusRectColor) : new SolidBrush(PlusRectColor);

                e.FillRectangle(minusplusBrush,
                    b.X - Indent + (float)Indent / 4,
                    b.Y + (float)b.Height / 4,
                    (float)Indent / 2,
                    (float)b.Height / 2);
            }
        }
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;
            var selectBounds = ShowLines == true ? e.Node.Bounds : e.Bounds;
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColors.PressColor),
                    selectBounds);

                TextRenderer.DrawText(e.Graphics, e.Node.Text, Font,
                     e.Node.Bounds, ForeColors.PressColor, TextFormatFlags.HorizontalCenter);
            }
            else if ((e.State & TreeNodeStates.Hot) != 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColors.HoverColor),
                  selectBounds);

                TextRenderer.DrawText(e.Graphics, e.Node.Text, Font,
                     e.Node.Bounds, ForeColors.HoverColor, TextFormatFlags.HorizontalCenter);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColors.DefaultColor),
                    selectBounds);

                TextRenderer.DrawText(e.Graphics, e.Node.Text, Font,
                   e.Node.Bounds, ForeColors.DefaultColor, TextFormatFlags.HorizontalCenter);
            }



            if (ShowLines)
            {
                var centerY = e.Node.Bounds.Y + e.Node.Bounds.Height / 2;
                var centerX = e.Node.Bounds.X;
                using var pen = new Pen(LineColor, 2);
                e.Graphics.DrawLine(pen,
                    new(centerX, centerY),
                    new(centerX - Indent / 2, centerY));
                var node = e.Node;
                if (node.Parent != null && node.Parent.FirstNode.Index == node.Index)
                {
                    e.Graphics.DrawLine(pen,
                       new(centerX - Indent / 2, node.Bounds.Y),
                       new(centerX - Indent / 2, centerY));
                }
                while (node != null)
                {

                    RepaintNodeLevel(node, e.Graphics, pen);
                    node = node.Parent;
                }

                //RepaintNodeLevel(node, e.Graphics, pen);
            }
            RepaintNodePlusMinus(e.Node, e.Graphics);
      
        }
    }
}
