﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System.IO;


namespace Mapping
{
    class RenderControl : UserControl
    {
        float cube_x = 1, cube_y = 1, cube_z = 1;

        private Font fontForDesignMode;
        SharpDX.Direct3D11.Device AppDevice;
        SharpDX.Direct3D11.DeviceContext context;
        RenderTargetView renderView = null;
        //Stopwatch clock;
        //Matrix view;
        //Matrix proj;
        SwapChain swapChain;
        Texture2D depthBuffer = null;
        DepthStencilView depthView = null;

        private SharpDX.Direct3D11.Buffer contantBuffer;
        private SharpDX.Direct3D11.Buffer verticesFront;
        private SharpDX.Direct3D11.Buffer verticesBack;
        private SharpDX.Direct3D11.Buffer verticesTop;
        private SharpDX.Direct3D11.Buffer verticesBottom;
        private SharpDX.Direct3D11.Buffer verticesLeft;
        private SharpDX.Direct3D11.Buffer verticesRight;
        private ShaderResourceView textureViewFront;
        private ShaderResourceView textureViewBack;
        private ShaderResourceView textureViewTop;
        private ShaderResourceView textureViewBottom;
        private ShaderResourceView textureViewLeft;
        private ShaderResourceView textureViewRight;
        string project_directory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

        //private Font fontForDesignMode;
        //SharpDX.Direct3D11.Device AppDevice;
        //SharpDX.Direct3D11.DeviceContext context;
        //private RenderTargetView backbufferview;
        //SwapChain swapChain;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderForm"/> class.
        /// </summary>
        public RenderControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
                base.OnPaintBackground(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode)
            {
                if (fontForDesignMode == null)
                    fontForDesignMode = new Font("Calibri", 24, FontStyle.Regular);

                e.Graphics.Clear(System.Drawing.Color.WhiteSmoke);
                string text = "SharpDX RenderControl";
                var sizeText = e.Graphics.MeasureString(text, fontForDesignMode);

                e.Graphics.DrawString(text, fontForDesignMode, new SolidBrush(System.Drawing.Color.Black), (Width - sizeText.Width) / 2, (Height - sizeText.Height) / 2);
            }

        }

        public void AlterTexture(string file) 
        {
            var textureBack = Texture2D.FromFile<Texture2D>(AppDevice, file);
            //var textureBack = Texture2D.FromFile<Texture2D>(AppDevice, "F:\\ISIB4\\Git_Kuka\\Mapping\\Mapping\\GeneticaMortarlessBlocks.jpg");
            textureViewBack = new ShaderResourceView(AppDevice, textureBack);

            var textureTop = Texture2D.FromFile<Texture2D>(AppDevice, file);
            textureViewTop = new ShaderResourceView(AppDevice, textureTop);

            var textureRight = Texture2D.FromFile<Texture2D>(AppDevice, file);
            textureViewRight = new ShaderResourceView(AppDevice, textureRight);
        
        }

        public void AlterTextureTop(string file)
        {
            var textureTop = Texture2D.FromFile<Texture2D>(AppDevice, file);
            textureViewTop = new ShaderResourceView(AppDevice, textureTop);
        }

        public void AlterTextureBack(string file)
        {
            var textureBack = Texture2D.FromFile<Texture2D>(AppDevice, file);
            textureViewBack = new ShaderResourceView(AppDevice, textureBack);
        }

        public void AlterTextureRight(string file)
        {
            var textureRight = Texture2D.FromFile<Texture2D>(AppDevice, file);
            textureViewRight = new ShaderResourceView(AppDevice, textureRight);
        }

        public void ReinitTexture()
        {
            var textureBack = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\back.jpg");
            textureViewBack = new ShaderResourceView(AppDevice, textureBack);

            var textureTop = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\top.jpg");
            textureViewTop = new ShaderResourceView(AppDevice, textureTop);

            var textureRight = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\right.jpg");
            textureViewRight = new ShaderResourceView(AppDevice, textureRight);

        }

        public void Render()
        {
            if (context != null)
            {
                // Prepare matrices
                //var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
                var view = Matrix.LookAtLH(new Vector3(3, 1, -5), new Vector3(0, 0, 0), Vector3.UnitY);
                Matrix proj = Matrix.Identity;

                // Setup new projection matrix with correct aspect ratio
                proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, this.ClientSize.Width / (float)this.ClientSize.Height, 0.1f, 100.0f);

                var viewProj = Matrix.Multiply(view, proj);

                // Clear views
                context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                context.ClearRenderTargetView(renderView, SharpDX.Color.Black);

                // Update WorldViewProj Matrix
                var worldViewProj = Matrix.RotationX(cube_x) * Matrix.RotationY(cube_y * 2) * Matrix.RotationZ(cube_z * .7f) * viewProj;
                worldViewProj.Transpose();
                context.UpdateSubresource(ref worldViewProj, contantBuffer);

                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesFront, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewFront);
                context.Draw(6, 0);
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesBack, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewBack);
                context.Draw(6, 0);
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesTop, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewTop);
                context.Draw(6, 0);
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesBottom, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewBottom);
                context.Draw(6, 0);
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesLeft, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewLeft);
                context.Draw(6, 0);
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesRight, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
                context.PixelShader.SetShaderResource(0, textureViewRight);
                context.Draw(6, 0);

                //context.Draw(36, 0);
                swapChain.Present(0, PresentFlags.None);

            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                InitGraphics();
            }
        }

        private void InitGraphics()
        {

            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(this.ClientSize.Width, this.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = this.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out AppDevice, out swapChain);
            context = AppDevice.ImmediateContext;

            // Ignore all windows events
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(this.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(AppDevice, backBuffer);

            

            //// Resize the backbuffer  (not present in MiniCubeTexture)
            //swapChain.ResizeBuffers(desc.BufferCount, this.ClientSize.Width, this.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

            //// (not present in MiniCubeTexture)
            //RasterizerStateDescription rasdesc = new RasterizerStateDescription()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.Solid,
            //    IsFrontCounterClockwise = true,
            //    DepthBias = 0,
            //    DepthBiasClamp = 0,
            //    SlopeScaledDepthBias = 0,
            //    IsDepthClipEnabled = true,
            //    IsMultisampleEnabled = true,
            //};
            //context.Rasterizer.State = new RasterizerState(AppDevice, rasdesc);

            // Compile Vertex and Pixel shaders
            var vertexShaderByteCode = ShaderBytecode.Compile(Properties.Resources.MiniCube, "VS", "vs_4_0");
            var vertexShader = new VertexShader(AppDevice, vertexShaderByteCode);

            var pixelShaderByteCode = ShaderBytecode.Compile(Properties.Resources.MiniCube, "PS", "ps_4_0");
            var pixelShader = new PixelShader(AppDevice, pixelShaderByteCode);

            // Layout from VertexShader input signature
            var layout = new InputLayout(
                AppDevice,
                ShaderSignature.GetInputSignature(vertexShaderByteCode),
                new[]
                    {
                        // First is a 4-float element tagged as "POSITION" in First.fx from SimpleColorVertex struct
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        // Second is a 4-float element tagged as "COLOR" in First.fx from SimpleColorVertex struct
						// Each float is 32 bits (4 bytes), so this element is 16 bytes from the start of the structure
                        //new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
                    });

            // Instantiate Vertex buiffer from vertex data
            verticesFront = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Front
                                      -1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                       1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 1.0f,
                                  });

            verticesBack = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      //-1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f, // BACK
                                      // 1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                                      //-1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                      //-1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      // 1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                      // 1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,

                                      -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f, // BACK
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                       1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                  });

            verticesTop = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      //-1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f, // Top
                                      //-1.0f, 1.0f,  1.0f,  1.0f,     0.0f, 0.0f,
                                      // 1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                                      //-1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f,
                                      // 1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                                      // 1.0f, 1.0f, -1.0f,  1.0f,     1.0f, 1.0f,

                                      -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 0.0f, // Top
                                      -1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                                       1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 1.0f,
                                      -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 0.0f,
                                       1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 1.0f,
                                       1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f,
                                  });

            verticesBottom = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f, // Bottom
                                       1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,
                                      -1.0f,-1.0f,  1.0f,  1.0f,     1.0f, 1.0f,
                                      -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f,
                                       1.0f,-1.0f, -1.0f,  1.0f,     0.0f, 0.0f,
                                       1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,
                                  });

            verticesLeft = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Left
                                      -1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 1.0f,
                                  });

            verticesRight = SharpDX.Direct3D11.Buffer.Create(AppDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                       //1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f, // Right
                                       //1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                                       //1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                       //1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                       //1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                                       //1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,

                                       1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Right
                                       1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                       1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                       1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                                       1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                  });

            // Create Constant Buffer
            //var contantBuffer = new SharpDX.Direct3D11.Buffer(AppDevice, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            contantBuffer = new SharpDX.Direct3D11.Buffer(AppDevice, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            // Create Depth Buffer & View
            depthBuffer = new Texture2D(AppDevice, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = this.ClientSize.Width,
                Height = this.ClientSize.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            var depthView = new DepthStencilView(AppDevice, depthBuffer);

            // Load texture and create sampler
            //var texture = Texture2D.FromFile<Texture2D>(AppDevice, "F:\\ISIB4\\Git_Kuka\\Mapping\\Mapping\\square.jpg");
            //var texture = Texture2D.FromFile<Texture2D>(AppDevice, "F:\\ISIB4\\Git_Kuka\\Mapping\\Mapping\\GeneticaMortarlessBlocks.jpg");
            var textureFront = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\front.jpg");
            var textureBack = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\back.jpg");
            var textureTop = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\top.jpg");
            var textureBottom = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\bottom.jpg");
            var textureLeft = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\left.jpg");
            var textureRight = Texture2D.FromFile<Texture2D>(AppDevice, project_directory + "\\right.jpg");
            //var textureView = new ShaderResourceView(AppDevice, texture);
            textureViewFront = new ShaderResourceView(AppDevice, textureFront);
            textureViewBack = new ShaderResourceView(AppDevice, textureBack);
            textureViewTop = new ShaderResourceView(AppDevice, textureTop);
            textureViewBottom = new ShaderResourceView(AppDevice, textureBottom);
            textureViewLeft = new ShaderResourceView(AppDevice, textureLeft);
            textureViewRight = new ShaderResourceView(AppDevice, textureRight);

            var sampler = new SamplerState(AppDevice, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                BorderColor = SharpDX.Color.Black,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16,
            });

            // Prepare All the stages
            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            //context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() * 2, 0));
            //Utilities.SizeOf<Vector4>() * 2 = 32
            //context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
            context.VertexShader.SetConstantBuffer(0, contantBuffer);
            context.VertexShader.Set(vertexShader);
            context.Rasterizer.SetViewport(new Viewport(0, 0, this.ClientSize.Width, this.ClientSize.Height, 0.0f, 1.0f));
            context.PixelShader.Set(pixelShader);
            context.PixelShader.SetSampler(0, sampler);
            //context.PixelShader.SetShaderResource(0, textureView);
            //context.OutputMerger.SetTargets(depthView, renderView);
            context.OutputMerger.SetTargets(renderView);

            //// Prepare matrices
            //var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
            //Matrix proj = Matrix.Identity;

            //// Setup new projection matrix with correct aspect ratio
            //proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, this.ClientSize.Width / (float)this.ClientSize.Height, 0.1f, 100.0f);

            //var viewProj = Matrix.Multiply(view, proj);



            this.KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.Right)
                {
                    cube_x = cube_x + 0.1f;
                }
                else if (args.KeyCode == Keys.Left)
                {
                    cube_x = cube_x - 0.1f;
                }
                else if (args.KeyCode == Keys.Up)
                {
                    cube_y = cube_y + 0.1f;
                }
                else if (args.KeyCode == Keys.Down)
                {
                    cube_y = cube_y - 0.1f;
                }
                else if (args.KeyCode == Keys.X)
                {
                    cube_z = cube_z + 0.1f;
                }
                else if (args.KeyCode == Keys.W)
                {
                    cube_z = cube_z - 0.1f;
                }
                Console.WriteLine(cube_x + " , " + cube_y + " , " + cube_z);

            };

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RenderControl
            // 
            this.Name = "RenderControl";
            this.Size = new System.Drawing.Size(258, 235);
            this.ResumeLayout(false);

        }
    }
}
