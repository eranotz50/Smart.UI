using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class XamlGridTest:PanelTestBase<FlexGrid>
    {
        [TestMethod]
        public void TwoRowsTest()
        {
            var panel = XamlReader.Load(@"
<FlexGrid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name='LayoutRoot' Background='#FF222121' DragEnabled='False'>
        <FlexGrid.RowDefinitions>
            <LineDefinition Length='30' />
            <LineDefinition  Length='1*'/>          
        </FlexGrid.RowDefinitions>
        <FlexGrid.ColumnDefinitions>
            <LineDefinition/>
        </FlexGrid.ColumnDefinitions> 
<Border Name='br1' FlexGrid.Row='0' FlexGrid.Column='0' />
<Border Name='br2' FlexGrid.Row='1' FlexGrid.Column='0' />

</FlexGrid>") as FlexGrid;
            panel.ShouldNotBeNull();
            if(panel==null) throw new NullReferenceException();
            this.Panel.AddChild(panel);
            panel.SetPlace(new Rect(0, 0, 1000, 1000));
            this.UpdateLayout();

            panel.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            panel.RowDefinitions.Count.ShouldBeEqual(2);
            panel.ColumnDefinitions.Count.ShouldBeEqual(1);            

            panel.RowDefinitions.Stars.Count.ShouldBeEqual(1);
            panel.RowDefinitions.Values.Count.ShouldBeEqual(1);
            
            panel.RowDefinitions[0].Value.ShouldBeEqual(30);
            panel.RowDefinitions[1].Stars.ShouldBeEqual(1);
            panel.RowDefinitions.StarLength.ShouldBeEqual(970);
            panel.RowDefinitions[1].Value.ShouldBeEqual(970);

            var br1 = panel.ChildByName<Border>("br1");
            var br2 = panel.ChildByName<Border>("br2");

            br1.GetRow().ShouldBeEqual(0);
            br1.GetColumn().ShouldBeEqual(0);
            br1.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, 30));

            br2.GetRow().ShouldBeEqual(1);
            br2.GetColumn().ShouldBeEqual(0);
            br2.GetBounds().ShouldBeEqual(new Rect(0, 30, 1000, 970));
        }

        public void AutoTest()
        {
               var panel =
                XamlReader.Load(@"           
<SmartGrid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name='LayoutRoot'  Width='1000' Height='1000'>
            <SmartGrid.ColumnDefinitions>
                <LineDefinition Length='auto'></LineDefinition>
                <LineDefinition Length='auto'></LineDefinition>
                <LineDefinition Length='auto'></LineDefinition>               
            </SmartGrid.ColumnDefinitions>

            <BlueLabel Name='qIcons' FlexGrid.Column='0'>Количество иконок</BlueLabel>
            <Slider  FlexGrid.Column='0' Name='qIconsSlider'   HorizontalAlignment='Left' Minimum='1' Maximum='60' Value='16'  Height='20' Width='200' VerticalAlignment='Top' />

            <BlueLabel  FlexGrid.Column='1'  Name='qFields'  FlexCanvas.Left='10' Width='150' Height='20'>Количество слотов</BlueLabel>
            <Slider  FlexGrid.Column='1'  Name='qFieldsSlider'     HorizontalAlignment='Left' Minimum='1' Maximum='60' Value='16'  Height='20' Width='200' VerticalAlignment='Top' />

            <BlueLabel FlexGrid.Column='2' Name='qTime'   Width='150' FlexCanvas.Left='10'>Время (секунды)</BlueLabel>
            <Slider  FlexGrid.Column='2' Name='qTimeSlider'   HorizontalAlignment='Left' Minimum='1' Maximum='60' Value='16'  Height='20' Width='200' VerticalAlignment='Top' />

            <Button  Name='playBtn' FlexGrid.Column='2' Relative.To='qTimeSlider'  FontSize='40' Content='Играть' Click='Button_Click_1'></Button>


        </SmartGrid>
") as FlexGrid;
            panel.ShouldNotBeNull();
            if (panel == null) throw new NullReferenceException();
            this.Panel.AddChild(panel);
            panel.SetPlace(new Rect(0, 0, 1000, 1000));
            this.UpdateLayout();
            
            panel.ColumnDefinitions.Count.ShouldBeEqual(3);
            panel.RowDefinitions.Count.ShouldBeEqual(0);
            var qIconsSlider = panel.ChildByName<Slider>("qIconsSlider");
            qIconsSlider.Width.ShouldBeEqual(200.0);
            panel.ColumnDefinitions[0].Value.ShouldBeEqual(200.0);

            qIconsSlider.SetWidth(300.0);
            this.Panel.UpdateLayout();
            panel.ColumnDefinitions[0].Value.ShouldBeEqual(300.0);

            qIconsSlider.SetWidth(190.0);
            this.Panel.UpdateLayout();
            panel.ColumnDefinitions[0].Value.ShouldBeEqual(190.0);

            

        }

        [TestMethod]
        public void AutoAndSpanTest()
        {
            var panel =
                XamlReader.Load(@"
<FlexGrid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name='LayoutRoot' Background='Transparent' Opacity='0.98' >
        <FlexGrid.RowDefinitions>
            <LineDefinition Length='20'></LineDefinition>
            <LineDefinition Length='*'></LineDefinition>
        </FlexGrid.RowDefinitions>
        <FlexGrid.ColumnDefinitions>
            <LineDefinition Length='auto' ></LineDefinition>
            <LineDefinition Length='*'></LineDefinition>
        </FlexGrid.ColumnDefinitions>

<Rectangle Fill='#007ACC'  Name='rec1'></Rectangle>
<Rectangle Fill='#007ACC' FlexGrid.ColumnSpan='2' Height='2' VerticalAlignment='Bottom' Name='rec2'></Rectangle>
        <TextBlock Name='Header' FontFamily='Arial'  Foreground='White'  FontSize='10'  VerticalAlignment='Center' HorizontalAlignment='Left' FontWeight='Bold' Margin='20,0' />        
</FlexGrid>") as FlexGrid;
            panel.ShouldNotBeNull();
            if(panel==null) throw new NullReferenceException();
            this.Panel.AddChild(panel);
            panel.SetPlace(new Rect(0, 0, 1000, 1000));
            this.UpdateLayout();
            var rec1 = panel.ChildByName<Rectangle>("rec1");
            var rec2 = panel.ChildByName<Rectangle>("rec2");
            rec1.GetColumn().ShouldBeEqual(0);
            rec2.GetRow().ShouldBeEqual(0);
            rec2.GetColumnSpan().ShouldBeEqual(2);
            var header = panel.ChildByName<TextBlock>("Header");
            //header


        }
        
    }
}
