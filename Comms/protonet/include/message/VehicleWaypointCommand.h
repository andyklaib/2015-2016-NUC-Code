/**@file This file was autogenerated. All changes will be undone. */

/** Message: Vehicle_Waypoint_Command, ID: 1006*/

#ifndef _VEHICLE_WAYPOINT_COMMAND_H_
#define _VEHICLE_WAYPOINT_COMMAND_H_

#include <protonet_marshal.h>
#include <protonet_message.h>

typedef struct {
   float64_t timestamp;
   uint16_t vehicle_ID;
   int32_t latitude;
   int32_t longitude;
   int32_t altitude;
   int32_t heading;
   uint8_t waypoint_ID;
   uint8_t waypoint_type;
} vehicle_waypoint_command_t;

msg_offset pack_vehicle_waypoint_command(
   vehicle_waypoint_command_t* vehicle_waypoint_command,
   msg_offset offset);

msg_offset unpack_vehicle_waypoint_command(
   msg_offset offset,
   vehicle_waypoint_command_t* out_ptr);

void encode_vehicle_waypoint_command(
   uint8_t src_id,
   uint8_t dest_id,
   uint8_t msg_ttl,
   uint8_t seq_number,
   vehicle_waypoint_command_t* tx_msg,
   proto_msg_t* msg);

#endif