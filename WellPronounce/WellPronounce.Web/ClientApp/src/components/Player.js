import React, { useState, useEffect } from "react";

const useAudio = url => {
    var [audio] = useState(new Audio(url));
    const [playing, setPlaying] = useState(false);

    const toggle = () => {
        if (url == "") {
            alert("No Audio present");
            return;
        }
        setPlaying(!playing);
    }
    //console.log(url);
    useEffect(() => {
        audio = new Audio(url);
        playing ? audio.play() : audio.pause();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },
        [playing]
    );

    useEffect(() => {
        audio.addEventListener('ended', () => setPlaying(false));
        return () => {
            audio.removeEventListener('ended', () => setPlaying(false));
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [playing]);

    return [playing, toggle];
};

const Player = ({ url }) => {
    const [playing, toggle] = useAudio(url);
    //console.log(url);
    return (
        <div>
            <button className="btn btn-primary" onClick={toggle}>{playing ? <i className="fas fa-pause"> Pause</i> : <i className="fas fa-play"> Play</i>}</button>
        </div>
    );
};

export default Player;