/**@file This file was autogenerated. All changes will be undone. */

/** Message: Vehicle_Global_Position, ID: 2002*/

#ifndef _VEHICLE_GLOBAL_POSITION_H_
#define _VEHICLE_GLOBAL_POSITION_H_

#include <protonet_marshal.h>
#include <protonet_message.h>

typedef struct {
   float64_t timestamp;
   uint16_t vehicle_ID;
   int32_t latitude;
   int32_t longitude;
   int32_t altitude;
   int32_t heading;
   int16_t x_speed;
   int16_t y_speed;
   int16_t z_speed;
} vehicle_global_position_t;

msg_offset pack_vehicle_global_position(
   vehicle_global_position_t* vehicle_global_position,
   msg_offset offset);

msg_offset unpack_vehicle_global_position(
   msg_offset offset,
   vehicle_global_position_t* out_ptr);

void encode_vehicle_global_position(
   uint8_t src_id,
   uint8_t dest_id,
   uint8_t msg_ttl,
   uint8_t seq_number,
   vehicle_global_position_t* tx_msg,
   proto_msg_t* msg);

#endif