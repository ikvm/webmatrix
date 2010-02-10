namespace Kingsoft.Blaze.WorldEditor.CustomPropertyGrid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;

	[Serializable()]public class CustomPropertyCollection: List<CustomProperty>, ICustomTypeDescriptor
	{
	    #region "Implements ICustomTypeDescriptor"
		
		public System.ComponentModel.AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}
		
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}
		
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}
		
		public System.ComponentModel.TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}
		
		public System.ComponentModel.EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}
		
		public System.ComponentModel.PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}
		
		public object GetEditor(System.Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}
		
		public System.ComponentModel.EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}
		
		public System.ComponentModel.EventDescriptorCollection GetEvents(System.Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}
		
		public System.ComponentModel.PropertyDescriptorCollection GetProperties()
		{
			return TypeDescriptor.GetProperties(this, true);
		}
		
		public System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
		{
			PropertyDescriptorCollection Properties = new PropertyDescriptorCollection(null);
			CustomProperty cp;
			foreach (CustomProperty tempLoopVar_CustomProp in this)
			{
				cp = tempLoopVar_CustomProp;
				if (cp.Visible)
				{
                    List<Attribute> attrs = new List<Attribute>();
					
					// Expandable Object Converter
					if (!cp.IsBrowsable)
					{
                        //attrs.Add(new TypeConverterAttribute(typeof(BrowsableTypeConverter)));
                        attrs.Add(new BrowsableAttribute(cp.IsBrowsable));
                    }
					
					// The Filename Editor
					if (cp.UseFileNameEditor == true)
					{
						attrs.Add(new EditorAttribute(typeof(UIFilenameEditor), typeof(UITypeEditor)));
					}
					
					// Custom Choices Type Converter
					if (cp.Choices != null)
					{
						attrs.Add(new TypeConverterAttribute(typeof(CustomChoices.CustomChoicesTypeConverter)));
					}
					
					// Password Property
					if (cp.IsPassword)
					{
						attrs.Add(new PasswordPropertyTextAttribute(true));
					}
					
					// Parenthesize Property
					if (cp.Parenthesize)
					{
						attrs.Add(new ParenthesizePropertyNameAttribute(true));
					}
					
					// Datasource
					if (cp.DataSource != null)
					{
						attrs.Add(new EditorAttribute(typeof(UIListboxEditor), typeof(UITypeEditor)));
					}
					
					// Custom Editor
					if (cp.CustomEditor != null)
					{
						attrs.Add(new EditorAttribute(cp.CustomEditor.GetType(), typeof(UITypeEditor)));
					}
					
					// Custom Type Converter
					if (cp.CustomTypeConverter != null)
					{
						attrs.Add(new TypeConverterAttribute(cp.CustomTypeConverter.GetType()));
					}

                    // Is Percentage
                    if (cp.IsPercentage)
                    {
                        attrs.Add(new TypeConverterAttribute(typeof(OpacityConverter)));
                    }
					
					// 3-dots button event delegate
					if (cp.OnClick != null)
					{
						attrs.Add(new EditorAttribute(typeof(UICustomEventEditor), typeof(UITypeEditor)));
					}
					
					// Default value attribute
                    if (cp.DefaultValue != null && cp.DefaultValue.ToString() != "")
					{
						attrs.Add(new DefaultValueAttribute(cp.Type, cp.Value.ToString()));
					}
					else
					{
						// Default type attribute
						if (cp.DefaultType != null)
						{
							attrs.Add(new DefaultValueAttribute(cp.DefaultType, null));
						}
					}
					
					// Extra Attributes
                    if (cp.Attributes != null)
                    {
                        foreach(Attribute attr in cp.Attributes)
                            attrs.Add(attr);
                    }
					
					// Add my own attributes
					//Attribute[] attrArray =  (System.Attribute[]) attrs.ToArray(typeof(Attribute));
                    Properties.Add(new CustomProperty.CustomPropertyDescriptor(cp, attrs.ToArray()));
				}
			}
			return Properties;
		}

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            return this;
        }			

	#endregion

	    #region "Serialize & Deserialize related methods"
	
	    public void SaveXml(string filename)
	    {
		    XmlSerializer serializer = new XmlSerializer(typeof(CustomPropertyCollection));
		    FileStream writer = new FileStream(filename, FileMode.Create);
		    try
		    {
			    serializer.Serialize(writer, this);
		    }
		    catch (Exception ex)
		    {
			    MessageBox.Show(ex.InnerException.Message);
		    }
		    writer.Close();
	    }
    	
	    public bool LoadXml(string filename)
	    {
		    try
		    {
			    XmlSerializer serializer = new XmlSerializer(typeof(CustomPropertyCollection));
			    FileStream reader = new FileStream(filename, FileMode.Open);
    			
			    CustomPropertyCollection cpc = (CustomPropertyCollection) serializer.Deserialize(reader);
			    foreach (CustomProperty customprop in cpc)
			    {
				    customprop.RebuildAttributes();
				    this.Add(customprop);
			    }
			    cpc = null;
			    reader.Close();
			    return true;
    			
		    }
		    catch (Exception)
		    {
			    return false;
		    }
    		
	    }
    	
	    public void SaveBinary(string filename)
	    {
		    Stream stream = File.Create(filename);
		    BinaryFormatter serializer = new BinaryFormatter();
		    try
		    {
			    serializer.Serialize(stream, this);
		    }
		    catch (Exception ex)
		    {
			    MessageBox.Show(ex.InnerException.Message);
		    }
		    stream.Close();
	    }
    	
	    public bool LoadBinary(string filename)
	    {
		    try
		    {
			    Stream stream = File.Open(filename, FileMode.Open);
			    BinaryFormatter formatter = new BinaryFormatter();
			    if (stream.Length > 0)
			    {
				    CustomPropertyCollection cpc = (CustomPropertyCollection) formatter.Deserialize(stream);
				    foreach (CustomProperty customprop in cpc)
				    {
					    customprop.RebuildAttributes();
					    this.Add(customprop);
				    }
				    cpc = null;
				    stream.Close();
				    return true;
			    }
			    else
			    {
				    stream.Close();
				    return false;
			    }
    			
		    }
		    catch (Exception)
		    {
			    return false;
		    }
        }

        #endregion	    
    }
}
