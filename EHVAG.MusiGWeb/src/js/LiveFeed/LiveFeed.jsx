import React, { PropTypes } from 'react';

const LiveFeed = React.createClass({
    render() {
        return (
          <div>
            <iframe
              width="560"
              height="315"
              src="https://www.youtube.com/embed/vIwZYYUir2U"
              frameBorder="0"
              allowFullScreen
            />
          </div>
        );
    },
});

export default LiveFeed;