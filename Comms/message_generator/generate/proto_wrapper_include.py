from lower_case_acronym import *

def generate_proto_wrapper_include(directory, include_extension, src_extension):
    """
    Protowrapper is used to create a Managed C which calls c++ 
    Function will create the protowrapper source file in .../Protowrapper
    File has 4 main parts
        -header struct
        -message struct
        -message callbacks
        -class node
            -public native methods
            -public send_message methods
            -public message call backs definitions
            -public register message definitions
            -private members
            -private message helper methods definitions            
    """
    #load header structure and the messages in variables from xml file
    import xml.etree.ElementTree as ET
    tree = ET.parse('message_definition.xml')
    protocol = tree.findall('message')
    header = tree.find('header')
    tab = '   '
    #open file
    f = open(directory + 'proto_wrapper' + include_extension, 'w')
    # warning
    f.write('/** @file This file was autogenerated. All changes will be undone.')
    f.write('\nWrapper Class is used to call c++ function through managed c++/cli for c# */\n\n')
    f.write('#ifndef _PROTO_WRAPPER_H_\n')
    f.write('#define _PROTO_WRAPPER_H_\n\n')    
    #include
    f.write('#include <stdio.h>\n')
    f.write('#include <vcclr.h>\n')
    f.write('#include <protonet'+include_extension+'>\n')
    f.write('#using <mscorlib.dll>\n\n')
    
    f.write('using namespace System;\n')
    f.write('using namespace System::Runtime::InteropServices;\n\n')
    
    #declare namespace
    f.write('namespace Protonet\n{\n')
    
    f.write(tab + "/** Redefines C++ header struct in c# any changes in c++ should also be made here*/ \n")
    f.write(tab+'public ref struct Header\n'+tab+'{\n')
    #create header struct
    for field in header:
        f.write(tab+tab+field.get('type')+' '+ field.get('name')+';\n')
    #header constructors
    f.write(tab+tab+'Header(){}\n')
    f.write(tab+tab+'Header(const Header% to_copy)\n')
    f.write(tab+tab+'{\n')
    for field in header:
        f.write(tab+tab+tab+field.get('name')+' = '+ 'to_copy.'+field.get('name')+';\n')
    f.write(tab+tab+'}\n')
    
    f.write(tab+tab+'Header(const proto_header_t to_copy)\n')
    f.write(tab+tab+'{\n')
    for field in header:
        f.write(tab+tab+tab+field.get('name')+' = '+ 'to_copy.'+field.get('name')+';\n')
    f.write(tab+tab+'}\n')
    f.write(tab+'};\n\n')
    
    #message structures
    for message in protocol:
        name = message.get('name')
        cs_name = message.get('name').replace("_","")
        variable_name = lower_case_acronym(name)
        type_t_name = variable_name + "_t"
        f.write(tab + "/** Redefine c++ " + name + " struct as c# struct which is auto generated*/ \n")
        f.write(tab+'public ref struct '+ cs_name+'\n'+tab+'{\n')
        for field in message:
            f.write(tab+tab+field.get('type')+' '+ field.get('name')+';\n')
        f.write(tab+tab+cs_name+'(){}\n')
        f.write(tab+tab+cs_name+'(const '+cs_name+'% to_copy)\n')
        f.write(tab+tab+'{\n')
        for field in message:
            f.write(tab+tab+tab+field.get('name')+' = '+ 'to_copy.'+field.get('name')+';\n')
        f.write(tab+tab+'}\n')
        
        f.write(tab+tab+cs_name+'(const '+type_t_name+' to_copy)\n')
        f.write(tab+tab+'{\n')
        for field in message:
            f.write(tab+tab+tab+field.get('name')+' = '+ 'to_copy.'+field.get('name')+';\n')
        f.write(tab+tab+'}\n')
        f.write(tab+'};\n\n')
    #message call backs
    f.write(tab + "/* Note [UnmanagedFunctionPointerAttribute(CallingConvention::Cdecl)] is used to set c# pointer type to Cdecl\n")
    f.write(tab + "which is the default c++ pointer type. If you get stack pointer error then the pointer type does not match c++ pointer type.*/ \n\n")
    for message in protocol:
        name = message.get('name')
        cs_name = message.get('name').replace("_", "")
        variable_name = lower_case_acronym(name)
        type_t_name = variable_name + "_t"
        f.write(tab + "[UnmanagedFunctionPointerAttribute(CallingConvention::Cdecl)]")
        f.write(tab + "/** Redefine c++ callback pointer as managed c++/cli pointer for c#.*/\n")
        f.write(tab+'public delegate void* '+cs_name+'Callback(int8_t, proto_header_t, '+type_t_name+', protonet::node*);\n')
    #class node
    f.write("\n" + tab + "/** Redefine c++ node class as c++/cli managed class for c#. Just look up c++ class for documentation.*/\n")
    f.write(tab+'public ref class Node\n'+tab+'{\n')
    f.write(tab+'public:\n')
    f.write(tab+tab+'Node(uint8_t node_id);\n')
    f.write(tab+tab+'~Node();\n')
    f.write(tab+tab+'void Start();\n')
    f.write(tab+tab+'void AddUDPDatalink([System::Runtime::InteropServices::Out]int8_t% link_id,uint16_t port);\n')
    f.write(tab+tab+'void AddUDPDatalink([System::Runtime::InteropServices::Out]int8_t% link_id,uint16_t port, String^ addr);\n')
    f.write(tab+tab+'void EstablishUDPEndpoint(int8_t link_id, uint8_t node_id, uint16_t port, String^ addr);\n')
    f.write(tab+tab+'void AddSerialDatalink([System::Runtime::InteropServices::Out]int8_t% link_id, uint32_t baud_rate, String^ device_path);\n')
    f.write(tab+tab+'void EstablishSerialEndpoint(int8_t link_id, uint8_t node_id);\n\n')
    
    #node send_message
    for message in protocol:
        name = message.get('name')
        cs_name = message.get('name').replace("_","")
        variable_name = lower_case_acronym(name)
        type_t_name = variable_name + "_t"
        f.write(tab+'void Send'+ cs_name+'(\n')
        f.write(tab+tab+'uint8_t dest_id,')
        for field in message:
            f.write('\n'+tab+tab+field.get('type')+' '+ field.get('name')+',')
        f.seek(-1, 1)
        f.write(');\n\n')
    #node message methods enter, exit, ping, ect
    for message in protocol:
        name = message.get('name')
        cs_name = message.get('name').replace("_", "")
        variable_name = lower_case_acronym(name)
        type_t_name = variable_name + "_t"
        f.write(tab+'delegate void '+cs_name+'Delegate(int8_t, Header^%, '+cs_name+'^%, Protonet::Node^);\n')
     
    f.write('\n')
    
    #node register methods
    for message in protocol:
       name = message.get('name')
       cs_name = message.get('name').replace("_", "")
       variable_name = lower_case_acronym(name)
       type_t_name = variable_name + "_t"
       f.write(tab+'void Register'+cs_name+'Event('+cs_name+'Delegate^ '+cs_name+'Event);\n')
    
    #node private members 
    f.write('\n'+tab+'private:\n')
    f.write(tab+tab+'protonet::node* node;\n\n')

    for message in protocol:
       name = message.get('name')
       cs_name = message.get('name').replace("_", "")
       variable_name = lower_case_acronym(name)
       type_t_name = variable_name + "_t"
       f.write(tab+tab+'IntPtr On'+cs_name+'Ptr;\n')
       f.write(tab+tab+cs_name+'Callback^ On'+cs_name+'Callback;\n')
       f.write(tab+tab+cs_name+'Delegate^ On'+cs_name+'Delegate;\n\n')
    #node message helper methods
    for message in protocol:
        name = message.get('name')
        cs_name = message.get('name').replace("_", "")
        variable_name = lower_case_acronym(name)
        type_t_name = variable_name + "_t"
        f.write(tab+tab+'void* '+cs_name+'Helper(int8_t link_id, proto_header_t header, '+type_t_name+' '+variable_name+', protonet::node* ptr);\n')
    f.write('\n'+tab+'};\n}\n#endif')
    
    print("Created: " + 'proto_wrapper' + src_extension)