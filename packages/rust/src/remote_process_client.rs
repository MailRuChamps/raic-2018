use serde::{Deserialize, Serialize};
use std::io::{self, BufReader, BufWriter};
use std::net::TcpStream;

pub struct RemoteProcessClient {
    buffer: String,
    reader: BufReader<TcpStream>,
    writer: BufWriter<TcpStream>,
}

impl RemoteProcessClient {
    pub fn connect(host: &str, port: u16) -> io::Result<Self> {
        let stream = TcpStream::connect((host, port))?;
        stream.set_nodelay(true)?;
        let stream_clone = stream.try_clone()?;
        let mut result = RemoteProcessClient {
            buffer: String::new(),
            reader: BufReader::new(stream),
            writer: BufWriter::new(stream_clone),
        };
        use std::io::Write;
        writeln!(result.writer, "json")?;
        result.writer.flush()?;
        Ok(result)
    }

    pub fn read<T: for<'a> Deserialize<'a>>(&mut self) -> io::Result<Option<T>> {
        use std::io::BufRead;
        self.buffer.clear();
        self.reader.read_line(&mut self.buffer)?;
        trace!("Read line: {:?}", self.buffer);
        if self.buffer.is_empty() {
            return Ok(None);
        }
        Ok(Some(::serde_json::from_str(&self.buffer)?))
    }
    pub fn write<T: Serialize>(&mut self, value: &T) -> io::Result<()> {
        use std::io::Write;
        writeln!(self.writer, "{}", ::serde_json::to_string(value)?)?;
        self.writer.flush()?;
        Ok(())
    }
    pub fn write_token(&mut self, token: &str) -> io::Result<()> {
        use std::io::Write;
        writeln!(self.writer, "{}", token)?;
        self.writer.flush()?;
        Ok(())
    }
}
