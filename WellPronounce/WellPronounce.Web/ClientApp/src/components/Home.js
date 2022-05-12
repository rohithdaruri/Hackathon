import React, { Component, useState } from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import Player from "./Player";
import createRecorder from "react-simple-recorder";
import axios from 'axios';
const Recorder = createRecorder(React);

export class Home extends Component {
    static displayName = Home.name;
    constructor(props) {
        super(props);

        this.state = {
            value: "",
            phoneticValue: "",
            copied: false,
            status: 1,
            standardLFNTextInput: "",
            standardLLNTextInput: "",
            standardPNTextInput: "",
            customLFNTextInput: "",
            customLLNTextInput: "",
            customPNTextInput: "",
        };

        this.handleStandardLFNChange = this.handleStandardLFNChange.bind(this);
        this.handleStandardLLNChange = this.handleStandardLLNChange.bind(this);
        this.handleStandardPNChange = this.handleStandardPNChange.bind(this);

        this.handleCustomLFNChange = this.handleCustomLFNChange.bind(this);
        this.handleCustomLLNChange = this.handleCustomLLNChange.bind(this);
        this.handleCustomPNChange = this.handleCustomPNChange.bind(this);

    }


    handleStandardLFNChange(event) {
        this.setState({
            standardLFNTextInput: event.target.value
        });
    }

    handleStandardLLNChange(event) {
        this.setState({
            standardLLNTextInput: event.target.value
        });
    }

    handleStandardPNChange(event) {
        this.setState({
            standardPNTextInput: event.target.value
        });
    }

    handleCustomLFNChange(event) {
        this.setState({
            customLFNTextInput: event.target.value
        });
    }

    handleCustomLLNChange(event) {
        this.setState({
            customLLNTextInput: event.target.value
        });
    }

    handleCustomPNChange(event) {
        this.setState({
            customPNTextInput: event.target.value
        });
    }

    radioHandler = (status) => {
        this.setState({ status });
        this.setState({ value: "" });
        this.setState({ phoneticValue: "" });
        this.setState({ standardLFNTextInput: "" });
        this.setState({ standardLLNTextInput: "" });
        this.setState({ standardPNTextInput: "" });
        this.setState({ customLFNTextInput: "" });
        this.setState({ customLLNTextInput: "" });
        this.setState({ customPNTextInput: "" });
    };

    convertOnClick = () => {

        if (this.state.standardLFNTextInput == "" || this.state.standardLLNTextInput == "") {
            alert("Legal FirstName and Legal LastnName are required");
            return;
        }

        document.getElementById('modal-root').style.filter = 'blur(5px)'
        var payload = {
            legalFirstName: this.state.standardLFNTextInput,
            legalLastName: this.state.standardLLNTextInput,
            preferedName: this.state.standardPNTextInput,
            language: "English",
            processType: "Standard"
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        };

        const response = fetch('/api/TextToSpeech/StandardProcess', requestOptions).then(r => r.json()).then(res => {
            if (res) {
                this.setState({
                    value: res.path
                });
                //console.log(this.state.value);
                document.getElementById('modal-root').style.filter = 'blur(0px)'
            }
        });

    }

    customProcess = (blobUrl, blob) => {

        if (this.state.customLFNTextInput == "" || this.state.customLLNTextInput == "") {
            alert("Legal FirstName and Legal LastnName are required, Please record again");
            return;
        }

        console.log(blobUrl);

        console.log(blob);// Need to send this blob to the API
       

        var payload = {
            legalFirstName: this.state.customLFNTextInput,
            legalLastName: this.state.customLLNTextInput,
            preferedName: this.state.customPNTextInput,
            language: "English",
            processType: "NonStandard"//,
            //audioFile: blob
        };

        
        const requestOptions = {
            method: 'POST',
            headers: { 'content-type': 'application/json' },
            body: JSON.stringify(payload)
        };

        const response = fetch('/api/TextToSpeech/CustomProcess', requestOptions).then(r => r.json()).then(res => {
            if (res) {
                this.setState({
                    value: res.path
                });
            }
        });


        //let data = new FormData();

        //data.append('legalFirstName', this.state.customLFNTextInput);
        //data.append('legalLastName', this.state.customLLNTextInput);
        //data.append('preferedName', this.state.customPNTextInput);
        //data.append('language', "English");
        //data.append('processType', "NonStandard");
        //data.append('audioFile', blob);

        //const config = {
        //    headers: { 'content-type': 'application/json' }
        //}

        //axios.post('/api/TextToSpeech/CustomProcess', data, config)
        //    .then(response => {
        //        console.log(response);
        //    })
        //    .catch(error => {
        //        console.log(error);
        //    });

    }

    render() {
        const { status } = this.state;

        const myDivstyle = {
            padding: "3px",
        };

        const myBoxstyle = {
            padding: "50px"
        };

        return (
            <div id="modal-root">
                {/*Standard or Custom Type Selection*/}
                <div className="row">
                    <div className="col-sm-5" style={myDivstyle}>
                        <p>
                            <b>Select the option based on your prefered approach</b>
                        </p>
                    </div>
                    <div className="col-sm-7" style={myDivstyle}>
                        <div className="form-check-inline">
                            <label className="form-check-label" for="standardradio">
                                <input type="radio" className="form-check-input" id="standardradio" name="optradio" value="Standard" checked={status === 1} onClick={(e) => this.radioHandler(1)} /><b>Standard</b>
                            </label>
                        </div>
                        <div className="form-check-inline">
                            <label className="form-check-label" for="customradio">
                                <input type="radio" className="form-check-input" id="customradio" name="optradio" value="NonStandard" checked={status === 2} onClick={(e) => this.radioHandler(2)} /><b>NonStandard</b>
                            </label>
                        </div>
                    </div>
                </div>
                <br />
                {/*Standard Section*/}
                {status === 1 &&
                    <div id="standardDiv" className="card shadow-lg bg-white rounded" style={myBoxstyle}>
                        {/* Input Text*/}
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Legal FirstName</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.standardLFNTextInput} onChange={this.handleStandardLFNChange} id="standardLFNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Legal LastName</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.standardLLNTextInput} onChange={this.handleStandardLLNChange} id="standardLLNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Prefered Name</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.standardPNTextInput} onChange={this.handleStandardPNChange} id="standardPNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Language</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" id="standardLanguageInput" disabled value="English" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>

                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <button className="btn btn-dark" id="standardPlay" onClick={this.convertOnClick}><i className="fas fa-external-link-alt"> Convert</i></button>
                            </div>

                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>

                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <Player
                                    url={this.state.value}
                                />
                            </div>
                        </div>
                        {/* Output Link*/}
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}><b>Phonetics</b></div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" value={this.state.phoneticValue} disabled />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}><b>Audio Link</b></div>

                            <div className="col-sm-7" style={myDivstyle}>
                                <textarea type="text" className="form-control" id="standardDataOutput" value={this.state.value}
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
                    <div id="customDiv" className="card shadow-lg bg-white rounded" style={myBoxstyle}>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Legal FirstName</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.customLFNTextInput} onChange={this.handleCustomLFNChange} id="customLFNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Legal LastName</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.customLLNTextInput} onChange={this.handleCustomLLNChange} id="customLLNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Prefered Name</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" autoComplete="off" className="form-control" value={this.state.customPNTextInput} onChange={this.handleCustomPNChange} id="customPNTextInput" />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>
                                <b>Language</b>
                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" id="customLanguageInput" disabled value="English" />
                            </div>
                        </div>


                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}>

                            </div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <Recorder
                                    containerClassName="my-recorder-container"
                                    Stop={<div>Stop</div>}
                                    Play={<p>Play</p>}
                                    Pause={<button>Pause</button>}
                                    Record={<div>Record</div>}
                                    Send={<div>Upload</div>}
                                    onSend={(blobUrl, blob) => {
                                        this.customProcess(blobUrl,blob);
                                    }}
                                />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}><b>Phonetics</b></div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" value={this.state.phoneticValue} disabled />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-3" style={myDivstyle}><b>Audio Link</b></div>
                            <div className="col-sm-7" style={myDivstyle}>
                                <textarea type="text" className="form-control" id="customDataOutput" value={this.state.value}
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
            </div>
        );
    }
}
