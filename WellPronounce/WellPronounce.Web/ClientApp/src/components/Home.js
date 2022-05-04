import React, { Component, useState } from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import Player from "./Player";
import createRecorder from "react-simple-recorder";
const Recorder = createRecorder(React);

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            value: '',
            copied: false,
            status: 1,
            standardTextInput: ""
        };

        this.handleChange = this.handleChange.bind(this);
    }


    handleChange(event) {
        this.setState({ standardTextInput: event.target.value });
    }

    //state = {
    //    value: '',
    //    copied: false
    //};

    radioHandler = (status) => {
        this.setState({ status });
    };

    convertOnClick = () => {
        //console.log(standardTextInput);
        //console.log(standardTextInput.value);
        var payload = {
            text: this.state.standardTextInput,
            language: "English",
            type: "Standard"
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        };
        fetch('/api/TextToSpeech', requestOptions)
            .then(response => console.log(response))
            .then(data => console.log(data));

    }

    render() {
        const { status } = this.state;

        const myDivstyle = {
            padding: "3px",
        };

        return (
            <div className="container">

                {/*Standard or Custom Type Selection*/}
                <div className="row">
                    <div className="col-sm-5">
                        <p>
                            <b>Select the option based on your prefered approach</b>
                        </p>
                    </div>
                    <div className="col-sm-7">
                        <div className="form-check-inline">
                            <label className="form-check-label" for="standardradio">
                                <input type="radio" className="form-check-input" id="standardradio" name="optradio" value="Standard" checked={status === 1} onClick={(e) => this.radioHandler(1)} /><b>Standard</b>
                            </label>
                        </div>
                        <div className="form-check-inline">
                            <label className="form-check-label" for="customradio">
                                <input type="radio" className="form-check-input" id="customradio" name="optradio" value="Custom" checked={status === 2} onClick={(e) => this.radioHandler(2)} /><b>Custom</b>
                            </label>
                        </div>
                    </div>
                </div>
                <br />

                {/*Standard Section*/}
                {status === 1 &&
                    <div id="standardDiv">

                        {/* Input Text*/}
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}>
                                <b>Text</b>
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <textarea type="text" className="form-control" value={this.state.standardTextInput} onChange={this.handleChange} id="standardTextInput" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <input type="text" className="form-control" id="standardLanguageInput" disabled value="English" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <button className="btn btn-dark" onClick={this.convertOnClick}>Convert</button>
                            </div>
                            <div className="col-sm-2" style={myDivstyle}>
                                <Player
                                    url="https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"
                                />
                            </div>
                        </div>
                        <br />

                        {/* Output Link*/}
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}><b>Link</b></div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" id="dataOutput" value={this.state.value}
                                    onChange={({ target: { value } }) => this.setState({ value, copied: false })} disabled />
                            </div>
                            <div className="col-sm-2" style={myDivstyle}>
                                <CopyToClipboard text={this.state.value}
                                    onCopy={() => this.setState({ copied: true })}>
                                    <button className="btn btn-success"><i className="fas fa-copy"></i></button>
                                </CopyToClipboard>

                                {this.state.copied ? <span style={{ color: 'red' }}> Copied.</span> : null}
                            </div>
                        </div>

                    </div>
                }

                {/*Custom Section*/}
                {status === 2 &&
                    <div id="customDiv">
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}>
                                <b>Text</b>
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <textarea type="text" className="form-control" id="customTextInput" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <input type="text" className="form-control" id="customLanguageInput" disabled value="English" />
                            </div>
                        <div className="col-sm-5" style={myDivstyle}>
                                <Recorder
                                    containerClassName="my-recorder-container"
                                    Stop={<div>Stop</div>}
                                    Play={<p>Play</p>}
                                    Pause={<button>Pause</button>}
                                    Record={<div>Record</div>}
                                    Send={<div>Upload</div>}
                                    onSend={(blobUrl, blob) => {
                                        alert("check console!");
                                        console.log("blob : ", blob);
                                        console.log("blobUrl : ", blobUrl);
                                    }}
                                />
                            </div>
                        </div>

                    </div>
                }
            </div>
        );
    }
}
