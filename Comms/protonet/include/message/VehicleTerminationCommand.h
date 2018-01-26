/**@file This file was autogenerated. All changes will be undone. */

/** Message: Vehicle_Termination_Command, ID: 1004*/

#ifndef _VEHICLE_TERMINATION_COMMAND_H_
#define _VEHICLE_TERMINATION_COMMAND_H_

#include <protonet_marshal.h>
#include <protonet_message.h>

typedef struct {
   float64_t timestamp;
   uint16_t vehicle_ID;
   uint8_t termination_mode;
} vehicle_termination_command_t;

msg_offset pack_vehicle_termination_command(
   vehicle_termination_command_t* vehicle_termination_command,
   msg_offset offset);

msg_offset unpack_vehicle_termination_command(
   msg_offset offset,
   vehicle_termination_command_t* out_ptr);

void encode_vehicle_termination_command(
   uint8_t src_id,
   uint8_t dest_id,
   uint8_t msg_ttl,
   uint8_t seq_number,
   vehicle_termination_command_t* tx_msg,
   proto_msg_t* msg);

#endif