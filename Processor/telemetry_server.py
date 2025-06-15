import asyncio
import websockets
import json
import logging

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='[%(asctime)s] %(levelname)s: %(message)s',
    datefmt='%Y-%m-%d %H:%M:%S'
)

async def handler(websocket, path):
    client = f"{websocket.remote_address[0]}:{websocket.remote_address[1]}"
    logging.info(f"Client connected: {client}")
    try:
        async for message in websocket:
            logging.debug(f"Received message from {client}: {message}")
            try:
                telemetry = json.loads(message)
                logging.info(f"Telemetry received from {client}:\n{json.dumps(telemetry, indent=2)}")
            except json.JSONDecodeError as e:
                logging.warning(f"Invalid JSON received from {client}: {e}")
            except Exception as e:
                logging.error(f"Unexpected error processing message from {client}: {e}")
    except websockets.exceptions.ConnectionClosed as e:
        logging.info(f"Connection closed by {client}: {e.code} - {e.reason}")
    except Exception as e:
        logging.error(f"Unexpected error with client {client}: {e}")
    finally:
        logging.info(f"Client disconnected: {client}")

async def main():
    host = "localhost"
    port = 8765
    logging.info(f"Starting WebSocket server on ws://{host}:{port}")
    server = await websockets.serve(handler, host, port)
    logging.info("Server started. Awaiting connections...")
    await server.wait_closed()

# Start the event loop
try:
    asyncio.run(main())
except KeyboardInterrupt:
    logging.info("Server shutdown requested by user.")
